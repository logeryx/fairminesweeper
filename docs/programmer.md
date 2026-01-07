# Programátorská dokumentace – Fair Minesweeper

## Popis hry

Fair Minesweeper je varianta klasické hry Minesweeper pro Windows Forms, která přidává novou mechaniku: **hra aktivně rozpozná, když hráč hádá** a

* pokud neexistuje tah s jistotou, **přeskupí miny** tak, aby hráč **netrefil** minu (odstranění náhodného elementu klasické hry),
* pokud hráč hádá, a **přitom existuje jistý tah s jistotou**, může buď **potrestat** – přeskupit miny tak, aby kliknuté pole **bylo minou**, nebo **upozornit** a dát volbu zrušení tahu.

Aplikace je rozdělená do WinForms oken (hlavní menu, hra, různé malé dialogy) a soubory s herní logikou (`Grid`, `Tile`).

## Struktura souborů a jejich role

Každý soubor odpovídá jedné významné třídě. Následuje popis funkce hlavních metod každé třídy. (pro podrobnější popis funkcí vizte komentáře ve zrojovém kódu)

### [Program.cs](src/FairMinesweeper/Program.cs)

Vstupní bod aplikace je tradičně funkce Main(), spravuje opětovné souštění a volání **Main menu** a **Game**.

* `Main()` vytvoří první instanci **hlavního menu**; následně v cyklu spouští hru znovu podle volby hráče „Restart/Do menu“ z dialogu Game Over. 
* `LaunchMenu()` spustí hlavní menu a načte z něj parametry (velikost, počet min, další herní možnosti) a vrátí strukturu `Options`.
* `LaunchGame(options)` vytvoří herní okno `Game` s vybranými volbami. Zároveň **povolí** obsluhu událostí (především kliknutí na políčka) přes `EventHandlers.Enable()`.
* `Restart()`/`GoToMenu()` jsou volané po konci hry a nastaví booly pro další iteraci smyčky.

### [MainMenu.cs](src/FairMinesweeper/MainMenu.cs)

WinForms **hlavní menu** (nastavení a spouštění hry).

* možnosti velikosti herního pole (Small/Medium/Large) a **Custom** se zapisují do `gridWidth`, `gridHeight`; textboxy se aktivují/deaktivují podle výběru.
* Obtížnost mění **procento min** (Easy 12%, Moderate 16%, Hard 20%, Custom dovoluje libovolně 1–100 %). Výpočet absolutního počtu min je `width*height*percentage/100`, s minimem 1. (počet min se během hry může měnit, protože přeskupování min často musí změnit jejich počet aby mohlo splnit požadavky odhalených polí.
* Volitelné úpravy chování hry: **Guess notification**, **Punish mode**, **Debug mode**. Každý nastaví interní bool, který se předá do hry. 
* Tlačítko **Start** nastaví `clickedStart` a zavře dialog, to je potřeba aby se hra nespustila i po vykřížkování okna menu.

### [Game.cs](src/FairMinesweeper/Game.cs)

Hlavní **herní okno**.

* Vytvoří `Grid` (jeho konstruktoru předá hráčovo nastavení) a zavolá `grid.Init()`. Následně **přidá všechny WinForms políčka** (`PictureBox`) do `Controls` (nutné aby o nich WinForms vědělo) a nastaví velikost okna podle rozměrů mřížky a obrázku tlačítek. (fixně 32x32 px) 
* `GameOver(bool win)` otevře dialog `GameOver` a podle volby uživatele požádá třídu `Program` o restart nebo návrat do menu. 
* `OpenDialog_GuessDetected()` otevírá dialog při zjištěném hádání (volitelné podle nastavení). Hráč musí potvrdit, že chce aby pokračovalo zpracování tahu.

### [GameOver.cs](src/FairMinesweeper/GameOver.cs)

**Dialog po konci hry** se dvěma volbami: Restart / Menu. Atributy `ClickedRestart` a `ClickedMenu` oznamují výsledek. Po uzavření okna instance zůstává aktivní a tyto atributy se dají přečíst.

### [GuessDetected.cs](src/FairMinesweeper/GuessDetected.cs)

**Dialog upozornění na hádání**. Atribut `Continue` podle volby hráče (ano/ne pokračovat) se dá přečíst po uzavření okna.

### [EventHandlers.cs](src/FairMinesweeper/EventHandlers.cs)

Centralizované **obsluhy kliknutí myši** pro políčka (PictureBox `Button1`).

* `OnPress/OnRelease` zajišťují vizuální odezvu levého tlačítka; pravé tlačítko okamžitě označí pole vlajkou. 
* `OnClick` provádí **odkrytí** levým tlačítkem.
* `Enable()/Disable()` globálně povolí/zakáže vstupy (při konci hry).

### [Grid.cs](src/FairMinesweeper/Grid.cs)

Herní logiky **na úrovni celé hry**, operace na všech políčkách naráz.

* Hlavní je 2D pole všech políček `Tile[,]`, dále obsahuje referenci na svůj `Game` formulář, udržuje **počítadla** pro detekci výhry: `flaggedTiles`, `flaggedMines`, a celkový počet min `minesCount`. 
* `Init()` vytvoří všechny `Tile`, náhodně **rozmístí miny** (`PlantMines()`), předá seznam sousedů (soused je definovaný jeko jedno z 8 sousedících políček) každému políčku a inicializuje ho (navíc volitelně odkryje miny v debug režimu). 
* Pomocné metody mřížky: `GetNeighbours()`, `InBounds()`, `ShowAllMines()`, `HideAllMines()`, `ShowAllNumbers()`.
* **Detekce a ovládání stavu hry**: `GameOver(win)` zakáže vstupy, ukáže zbylé miny a předá řízení třídě `Game`. `CheckForWin()` detekuje vítězství: všechny miny a zároveň jen miny jsou označené vlajkou, poté odkryje všechna zbylá čísla a ukončí hru. 
* **Hledání „edge“ (krajních) dlaždic**: `GetEdgeTiles()` vrátí seznam **neodkrytých sousedů již odkrytých polí** (skrytá políčka o kterých má hráč nějakou informaci) – to slouží jako fronta pro přemísťování min v okraji viditelné oblasti. 
* **Přemísťování min (nová mechanika)**: `RearrangeEdgeBombs(edges, targetTile, targetEndsUpMine)` – prochází všechny kombinace hypotetických stavů krajních dlaždic tak, aby zachoval konzistenci čísel u odkrytých dlaždic a zároveň donutil cílovou dlaždici být **mina/nemina** podle potřeby. Používá rekurzivní backtracking `TryAllStatesFromIndex` napříč hypotetickými stavy. Po nalezení konzistentního rozložení aplikuje změny (`ApplyHypothetical()`), resetuje hypotézy a přepočítá okolní čísla pro skrytá pole. V **debug modu** znovu vykreslí miny. 
* **Hledání tahu s jistotou**: `SearchForCertainOption()` prohledá celou mřížku a zjišťuje, zda **existuje jistý tah** pomocí `Tile.Certain()` (pole 100% musí být mina nebo 100% není minou). `GuessDetected()` volá (dle volby v menu) potvrzovací dialog. 
* **Údržování počítadel**: `IncrementFlagged/DecrementFlagged()` spravují počítadla vlajek a min; `NumberOfMinesChanged()` aktualizuje celkový počet min (důležité při přeskupování). 

### [Tile.cs](src/FairMinesweeper/Tile.cs)

**Herní políčko** a jeho reprezentace, obsluha.

* Pomocná třída `Button1` – rozšíření WinForms třídy `PictureBox` o odkaz na svůj `Tile`. Každá `Tile` vlastní instanci `Button1`. Při `Init()` se nastaví pozice, rozměr, výchozí obrázek a **napojí se event handlery** z `EventHandlers`. 
* Stavy `Tile`: `mine`, `uncovered`, `flagged`, `surroundingMines`, `neighbours` popisují jestli je toto pole minou, odhalené, jaké číslo zobrazí po odkrytí a seznam až 8 okolních polí.
* **Hypotetický stav** (pro určení hádání a přeskupení min): `hypotheticallyFlagged`, `hypotheticallyUncovered` + odvozená vlastnost `TrueBlank`. Slouží jako neoficiální poznamenání, aby se měnilo reálné `flagged` a `uncovered` jen když k těmto změnám opravdu dochází. Obnovuje se přes `ResetHypothetical()`; fyzicky se aplikuje na realitu přes `ApplyHypothetical()`.
* **Značení vlajkou**: `ToggleFlagged()` mění obrázek a volá `Grid.(In|De)crementFlagged`. 
* **Odkrytí**: `Uncover(bool playerClick)` – při kliknutí hráče nejprve **zkontroluje hádání** (`CheckForGuessing()`); když jde o minu, ukončí hru; jinak zobrazí číslo a případně odhalí i sousedy u nuly. 

#### „Certainty logic“ (rozpoznání hádání matematickým sporem)

Mechanismus, který **z informací viditelných hráči** zjišťuje, zda je dlaždice **100% mina** nebo **100% bezpečná**. Postupuje se sporem, např. v prvním případě: buď toto pole ne-mina, hypoteticky vylušti vše co můžeš, když narazíš na spor s některým odhaleným políčkem, tak toto pole je 100% mina.

* Pro dlaždice se vyhodnocují **neodkryté sousední pozice** a **počty zbývajících min** tak, aby **neporušily** čísla u všech odkrytých dlaždic.
* Klíčové pomocné proměnné (odvozené v `UpdateNeighbourAtributes()`):

  * `remainingCoveredNeighbours`, `remainingMinesToBeFlagged`, vlastnosti `Illegal`, `Completed`, `SatisfiedUnfinished`, `OnlyMinesLeft`. (podrobně popsaní v zdrojovém kódu)

* `HypotheticallySolve()` průběžně doplňuje hypotetické stavy odvoditelné z původního předpoklad (odkrytelné/vlaječka - mina) a každou hypotetickou změnu dává je do společné fronty `Grid.BFSqueue` na pozdější prozkoumání k sporu a dalšímu doplnění. Jakmile narazí na `Illegal` pole (spor s pravidly hry), vrací `false`. 
* `Certain(invertujPředpokladSporu)` vyzkouší **jednu z možností** (např. „dlaždice je mina“) a šíří důsledky přes `BFSqueue` ("prozoumávání" herní mřížky se šíří podobně jako BFS, proto pojmenování fronty, ale nejedná se o klasické BFS) ; pokud dojde ke **sporu**, vrátí `true` → daný předpoklad **není možný**, tudíž platí **opak** (dlaždice je jistě bezpečná/mina). 

#### Detekce hádání a přeskupování min (spolupracují třídy `Tile` × `Grid`)

* `Tile.CheckForGuessing()` testuje, jestli je dlaždice **kompatibilní s oběma scénáři** (mina i bezpečná): tj. `!Certain(false) && !Certain(true)`. Pokud ano, jde o **hádání**. 
* Pokud zároveň existuje **jiný 100 procentní tah** (`Grid.SearchForCertainOption()` vrátí true):

  * buď pouze **upozorní** (dle nastavení),
  * nebo v **Punish mode** vynutí, aby kliknutá dlaždice byla minou (`RearrangeEdgeBombs(..., targetEndsUpMine:true)`). 

* Když **žádný jistý tah není**, `Grid.RearrangeEdgeBombs(..., targetEndsUpMine:false)` přeuspořádá krajní mini tak, aby kliknutá byla **bezpečná** a všechna čísla zůstala konzistentní, počty min opravdu seděli. 

## Tok událostí při kliknutí

1. Uživatel klikne levým tlačítkem na `PictureBox` (`Button1`). `EventHandlers.OnClick()` volá `Tile.Uncover(true)`. 
2. `Tile.Uncover(true)` → `CheckForGuessing()` → podle zjištění:

   * tah s jistotou: pokračuje standardně,
   * hádání + existuje jiný jistý tah: volitelné upozornění; v Punish módu se přeskupí miny proti hráči,
   * hádání + jiný jistý tah neexistuje: přeskupí miny ve prospěch hráče. 
   
3. Pokud je dlaždice mina, zobrazí se mina s červeným pozadím a volá `Grid.GameOver(win=false)`; jinak se ukáže číslo a při nule se odkryjí i sousedi. 
