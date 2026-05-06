# smartLockerFinder API

## Opis
Projekt **smartLockerFinderApi** to aplikacja w architekturze Clean Architecture/CQRS stworzona w platformie .NET 10. Służy do pobierania i filtrowania danych o paczkomatach (np. sieci InPost). Posiada wystawione API do integracji.

## Struktura projektu

Zastosowano podział na warstwy:
- **Api** - Kontrolery (np. `ParcelLockerController`), odpowiedzialne za przyjmowanie żądań HTTP i zwracanie odpowiedzi.
- **Application** - Logika aplikacji. Zawiera m.in. implementację wzorca CQRS za pomocą biblioteki MediatR (np. `FetchParcelLockerDataRequest`, `FetchParcelLockerDataRequestHandler`), a także obiekty DTO i odpowiedzi.
- **Domain** - Domena biznesowa. Zawiera definicje encji (np. `LocationData`, `AddressDetails`, `LockerFunctionsFilter`), serwisy domenowe (np. `ParcelLockerService`) oraz interfejsy (np. `IParcelLockerService`).

Oprócz tego w rozwiązaniu znajduje się projekt z testami: `smartLockerFinderApiTests`, gdzie umieszczono testy integracyjne i jednostkowe.

## Główne funkcjonalności
- Komunikacja z zewnętrznym API paczkomatów (za pomocą `HttpClient`).
- Wyszukiwanie, filtrowanie i pobieranie danych o paczkomatach.
- Zaimplementowany mechanizm CORS, pozwalający domyślnie na odpytywanie z `http://localhost`.

## Wymagania
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Jak uruchomić (lokalnie)

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/MichuZabba/smartLockerFinder-Inpost.git
   cd smartLockerFinder
   ```

2. Przejdź do katalogu lub otwórz rozwiązanie w Visual Studio:
   ```bash
   cd smartLockerFinderApi
   ```

3. Uruchom aplikację:
   ```bash
   dotnet run
   ```
   Alternatywnie: wybierz projekt startowy w środowisku Visual Studio i kliknij "Uruchom" (F5 dla trybu Debug).

## Jak uruchomić (Docker)

Z głównego katalogu repozytorium (tam gdzie znajduje się plik `docker-compose.yml`) możesz łatwo zbudować i uruchomić całe środowisko wraz z frontendem:

1. Zbuduj i uruchom kontenery za pomocą Docker Compose:
   ```bash
   docker-compose up --build
   ```
   Aby uruchomić w tle (detached mode), użyj flagi `-d`:
   ```bash
   docker-compose up -d
   ```

2. Aplikacja powinna być dostępna z poziomu przeglądarki pod odpowiednimi portami (zdefiniowanymi w pliku `docker-compose.yml`, np. http://localhost:8080 dla odpowiednich części aplikacji).

Aby zatrzymać kontenery:
```bash
docker-compose down
```

## Jak korzystać z API

Po włączeniu środowiska deweloperskiego (np. poprzez `dotnet run` lub używając Docker), API będzie gotowe do przyjmowania żądań.

1. **Endpoint `POST /api/ParcelLocker/Fetch`**
   Zewnętrznym aplikacjom udostępniony jest endpoint `Fetch`, za pomocą którego możemy pobrać dane o paczkomatach, podając lokalizację użytkownika (szerokość i długość geograficzna) oraz ustawione filtry.
   Przykładowy cURL (port dostosuj do wymagań):
   ```bash
   curl -X POST http://localhost:<PORT>/api/ParcelLocker/Fetch \
     -H "Content-Type: application/json" \
     -d '{
       "request": {
         "filterFunctions": {
           "returnEnabled": true,
           "allegroDelivery": false
         },
         "location": {
           "longitude": 19.944981,
           "latitude": 50.06465
         }
       }
     }'
   ```

## Uruchamianie testów
```bash
dotnet test
```

## Architektura i technologie
- **.NET 10**
- **CQRS / MediatR** - do oddzielenia komend/żądań.
- **Dependency Injection** (ASP.NET Core).
- **HttpClientFactory** - wstrzykiwanie klientów HTTP z konfigurowalnym zarządzaniem cyklem życia.
