# Kursy walut z NBP

## Podstawowe informacje

Prosty program służący do pobierania kursów walut z serwerów Narodowego Banku Polskiego w formacie XML i wyświetlania podstawowych statystyk dla kursu kupna i sprzedaży w okreslonym przedziale czasowym.


## Wyświetlane dane

* kurs średni (średnia arytmetyczna)
* odchylenie standardowe (_Standard Deviation_)
* kurs minimalny oraz dzień wystąpienia
* kurs maksymalny oraz dzień wystąpienia

## Obsługiwane waluty

* USD
* EUR
* CHF
* GBP

## Warianty

Program zrealizowany został w wariancie konsolowym oraz jako aplikacja okienkowa (WPF).

1. Pobranie danych w wariancie konsolowym - parametry przekazywane w linii komend:

    ```>Program.exe <kod waluty> <zakres od> <zakres do>```
  
    np.:
  
    ```>Program.exe EUR 2022-07-01 2022-07-07```
  
    Dane zostaną wyświetlone w konsoli.
  
2. Pobranie danych w wariancie okienkowym:

    Należy uzupełnić zakres oraz wybrać walutę, dane zostaną wyświetlone w odpowiednich polach.
