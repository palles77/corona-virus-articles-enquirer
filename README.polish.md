# Corona Virus Covid-19 Enquirer

Jest to projekt polegający na przeszukaniu 33033 artykułów naukowych dostarczonych przez Światową Organizację Zdrowia, aby pomóc naukowcom z całego świata w uzyskaniu szybkiego dostępu do najnowszych osiągnięć medycyny i nauki.

https://pages.semanticscholar.org/coronavirus-research

Ta praca jest całkowicie bezpłatna dla każdego rodzaju użytku akademickiego lub zawodowego.
To tylko mój mały sposób na powstrzymanie wirusa Corona.
Ten projekt będzie aktualizowany w miarę upływu czasu, dlatego prosimy często sprawdzać tę stronę internetową pod kątem udpates.

# Ważna uwaga
Ten program jest w wersji 64-bitowej i zużywa około 2 GB pamięci RAM, więc komputer prawdopodobnie powinien mieć co najmniej 4 GB pamięci RAM lub więcej.

# Aby skorzystać z tego projektu
Iść do:

https://github.com/palles77/corona-virus-articles-enquirer/releases/download/1.0.0.1/CovidEnquirerSetup-1.0.0.2.exe

Zainstaluj go, poczekaj na skonfigurowanie bazy danych artykułów, a następnie rozpocznij wyszukiwanie.

Program obsługuje:
1. Wyszukiwanie słów kluczowych.
2. Wyszukiwanie wybranego artykułu w Google.
3. Otwieranie wybranego artykułu w Word Pad
4. Zapisanie wybranego artykułu jako pliku Word.

# Plany rozszerzenia obejmują:
1. Włączenie silnika Lucene.Net, aby umożliwić dokładniejsze wyszukiwanie z wynikiem trafień.
2. Możliwość tłumaczenia artykułów na inne języki.
3. Możliwość zadawania bardziej ludzkich pytań.

# Robaki
Można zgłaszać błędy i prośby o nowe funkcje
https://github.com/palles77/corona-virus-articles-enquirer/issues

# Jak używać

1. Wystarczy zainstalować z
https://github.com/palles77/corona-virus-articles-enquirer/releases/download/1.0.0.1/CovidEnquirerSetup-1.0.0.2.exe
2. Po uruchomieniu instalacji program „Covid Enquirer 1.0.0.2” zostanie uruchomiony automatycznie. Można go również uruchomić z ikony na pulpicie lub wpisując „Covid Enquirer” z panelu wyszukiwania w systemie Windows.
Powinieneś zobaczyć następujący obraz:
![Ładowanie bazy danych](https://github.com/palles77/corona-virus-articles-enquirer/blob/master/Images/CovidEnquirerStartup.png)
3. Po kilku minutach oczekiwanie baza danych zakończy się ładowaniem (baza danych jest dość duża, ponieważ zawiera szczegóły dotyczące ponad 29 000 artykułów z całego świata - w języku angielskim).
Obraz, który zobaczysz, będzie mniej więcej taki:
![Baza danych załadowana](https://github.com/palles77/corona-virus-articles-enquirer/blob/master/Images/CovidEnquirerLoaded.png)
4. Określ kryteria wyszukiwania i naciśnij przycisk „Szukaj”. Podając więcej niż 1 słowo, program wyszuka artykuły zawierające wszystkie słowa.
Przykładowy obraz kryteriów wyszukiwania „szczepionka przeciw wirusowi krowianki”
![Określanie kryteriów wyszukiwania](https://github.com/palles77/corona-virus-articles-enquirer/blob/master/Images/CovidEnquirerSearching.png)
5. Po zakończeniu wyszukiwania zobaczysz listę artykułów w polu listy po lewej stronie. Zawiera posortowaną listę tytułów alfabetycznie. Aby zobaczyć treść artykułu wystarczy kliknąć tytuł, a zobaczysz prosty podgląd artykułów w formacie RTF.
Przykładowy artykuł można zobaczyć poniżej:
![Podgląd wybranego artykułu](https://github.com/palles77/corona-virus-articles-enquirer/blob/master/Images/Images/CovidEnquirerSearchingResult.png)
6. Możesz przetłumaczyć wybrany artykuł za pomocą Google na swój język. Dostępne języki to: chiński, czeski, francuski, niemiecki, grecki, włoski, japoński, polski, portugalski,
 Rosyjski, hiszpański Pamiętaj, że tłumaczenie zajmuje kilka minut z powodu ograniczeń nałożonych przez Google na ich bezpłatną usługę tłumaczeniową. Jednak tłumaczenia są całkiem dobre z naukowego punktu widzenia.
![Tłumaczenie artykułu](https://github.com/palles77/corona-virus-articles-enquirer/blob/master/Images/Images/CovidEnquirerTranslatedInPolish.png)

# Inne funkcjonalizacje
Program oferuje także menu kontekstowe dla każdego z artykułów, które pozwala:
* wyszukiwanie artykułu w Google poprzez uruchomienie przeglądarki z wyszukiwaniem skupia się na tytule bieżącego artykułu.
* zapisanie artykułu jako dokumentu Word.
* otwarcie artykułu w Word Pad.
* tłumaczenie artykułów.
* możliwość przesuwania paska podziału, aby zwiększyć pole listy. Możesz także dowolnie zmieniać rozmiar okna, dzięki czemu obszar do czytania może być większy lub mniejszy w zależności od życzeń użytkownika.

# Dziękuję notatki
Chciałbym podziękować za następujące projekty:
1. https://pages.semanticscholar.org/coronavirus-research
2. https://www.codeproject.com/articles/98062/rtf-document-constructor-library?fid=1581440&df=90&mpp=25&prof=True&sort=Position&view=Normal&spc=Relaxed&fr=51
3. https://www.newtonsoft.com/json
4. https://github.com/OfficeDev/Open-XML-SDK

# Kontakt
silesiaresearch at gmail dot com