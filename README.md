# Inwentaryzator paczkomatów

Repozytorium zawiera projekt do wyszukiwania / inwentaryzacji paczkomatów (smart lockerów) z podziałem na:
- **Frontend**: aplikacja webowa `smartLockerFinderFront` (React + Vite + Leaflet / React-Leaflet)
- **Backend/API**: katalog `smartLockerFinderApi` (API + testy)

---

## Struktura projektu

- `smartLockerFinderFront/` – aplikacja kliencka (mapa + UI)
- `smartLockerFinderApi/` – część serwerowa (API) oraz testy
  - `smartLockerFinderApi/` – właściwy projekt API
  - `smartLockerFinderApiTests/` – testy API

---

## Wymagania

### Frontend (`smartLockerFinder`)
- Node.js + npm (lub pnpm/yarn)
- Przeglądarka internetowa

---

## Uruchomienie frontendu (smartLockerFinder)

Wejdź do katalogu aplikacji i zainstaluj zależności:

```bash
cd smartLockerFinder
npm install
```

Uruchom tryb developerski:

```bash
npm run dev
```

Build produkcyjny:

```bash
npm run build
```

Podgląd builda:

```bash
npm run preview
```

Lint:

```bash
npm run lint
```

---

## Technologie (frontend)

- React
- Vite
- TypeScript
- Leaflet + React-Leaflet (mapa)

---

## Backend / API

Część API znajduje się w katalogu:

- `smartLockerFinderApi/`

W tym repozytorium jest też katalog testów API:

- `smartLockerFinderApi/smartLockerFinderApiTests/`

