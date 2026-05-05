# Inwentaryzator paczkomatów

Repozytorium zawiera projekt do wyszukiwania / inwentaryzacji paczkomatów (smart lockerów) z podziałem na:
- **Frontend**: aplikacja webowa `smartLockerFinder` (React + Vite + Leaflet / React-Leaflet)
- **Backend/API**: katalog `Inwentaryzator_paczkomatow_Api` (API + testy)

---

## Struktura projektu

- `smartLockerFinder/` – aplikacja kliencka (mapa + UI)
- `Inwentaryzator_paczkomatow_Api/` – część serwerowa (API) oraz testy
  - `Inwentaryzator_paczkomatow_Api/` – właściwy projekt API
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

- `Inwentaryzator_paczkomatow_Api/`

W tym repozytorium jest też katalog testów API:

- `Inwentaryzator_paczkomatow_Api/smartLockerFinderApiTests/`

