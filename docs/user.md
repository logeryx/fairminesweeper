# Uživatelská dokumentace – Fair Minesweeper

## Co je to Fair Minesweeper

Varianta klasického Minesweeperu, která se snaží odstranit element náhody „hádání“. Když hra rozpozná, že **neexistuje 100 procentní tah**, **přeskupí** miny tak, aby vaše kliknutí **nebylo minou** (spravedlivé dohrání bez štěstí). Pokud však **hádate** a přitom **existuje jistý tah**, můžete (dle nastavení hry) být **potrestáni** – kliknuté pole se po přeskupení stane minou, nebo jen informováni, že existují determinovatelná políčka.

## Spuštění

* Spusťte aplikaci [FairMinesweeper.exe](bin/FairMinesweeper.exe). Otevře se **Hlavní menu** hry.

*Program vypisuje pár informací do konzole. Program můžete spustit s konzolí např. pomocí PowerShellu `.\FairMinesweeper.exe | echo`*

## Ovládání

* **Levé tlačítko myši**: odkryje pole, může také vybouchnout minu.
* **Pravé tlačítko myši**: nastaví/odebere **vlaječku** (označení podezřelé miny).
* Cílem je: **označit všechny a pouze miny vlajkou** a **nevybouchnout odhlením miny**. Když jsou všechny miny správně označeny (a žádná vlajka není na bezpečném poli), zvítězíte.

## Main menu – nastavení

### Velikost mřížky

* **Small (9×9)**, **Medium (15×15)**, **Large (23×23)** rozměry v počtu políček.
* **Custom** – zadáte výšku a šířku. Neplatné hodnoty se automaticky vrátí na výchozích 15.

### Obtížnost (počet min)

* **Easy 12%**, **Moderate 16%**, **Hard 20%** – přibližné procentuální zastoupení min.
* **Custom** – vlastní procento (**1–100%**). Počet min se počítá jako `šířka × výška × procento / 100` (minimálně 1 mina). V případě neplatných hodnot se vrátí na výchozích 16%.

### Další možnosti

* **Notifikace při hádání** *(doporučeno)* — Pokud hra rozpozná, že **hádáte** dá vám možnost vzít tah zpět:

  * zobrazí **upozornění** a můžete rozhodnout, zda pokračovat;
  * když je vypnuto, hra plynule pokračuje bez dialogu.
  * toto nastavení je vhodné pro edukační záměry, nováčky ve hře
  
* **Punish guesses** — Obtížnější verze hry. Když hádáte a **existuje jiný jistý tah**, hra může přeskupit miny **proti vám** – **kliknuté pole se stane minou**. Určit kdy existuje 100% může být velmi obtížné.
* **Debug mode** — Zobrazí **polohy min** (vhodné pro testování a ladění, nebo jen pro zajímavost, jak se hýbají miny na pozadí).

### Hru spustíte tlačítkem START nebo stisknutím ENTER klávesy

## Jak funguje „férovost“

* Pokud **neexistuje žádný** stoprocentní tah, hra přeuspořádá miny kolem odkrytých dlaždic tak, aby **vaše kliknuté pole bylo bezpečné** a ani jste nevěděli, že se to stalo. (z pohledu hráče má jen obrovské štěstí)
* Pokud **existuje** jistý tah a vy přesto kliknete na pole, které si lze vyložit oběma způsoby (mina/bezpečné), může vás hra (podle nastavení) **upozornit** nebo i **potrestat** přeskupením min proti vám.

## Konec hry

* **Výhra**: všechny miny jsou správně označeny a žádná vlajka není na bezpečném poli. Hra automaticky odkryje zbývající čísla a ukáže dialog **Game Over**.
* **Prohra**: odkryjete minu. Hra ukáže všechny zbylé miny a dialog **Game Over**.
* V dialogu **Game Over** můžete zvolit **Restart** (nová hra se stejným nastavením) nebo **Menu** (návrat do hlavního menu a případná změna nastavení).

## Tipy

* **Guess notification** pomáhá učit se rozpoznávat situace, kde je tažení jisté vs. nejisté, kdykoli si nejste jisti jestli můžete hádat, tak při této volbě zapnuté se můžete ujistit kliknutím "do prázdna". Nemůže se vám nic stát.
* S touto volbou tedy můžete prohrát jen kliknutím na pole, které je dle dostupných informací 100% mina. Kdykoli se toto stane, tak jste měli jak na to přijít. Nelze prohrát náhodou, jen chybou.
* když některé políčko chybně označíte vlaječkou, může se vám stát, že vás hra nepodrží, procuje totiž se stejnými informacemi, jako vy, bere každou vaší vlajku za pravdu.

