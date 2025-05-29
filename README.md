# WPF-Shop – En butik i WPF
## 📌 Beskrivning
Den här WPF-applikationen representerar en butik där användare kan registrera sig som Admin eller Customer, logga in och hantera produkter. Vid programstart läses produktlistan och användarlistan in från filer i AppData/Local. Om dessa filer inte finns, skapas de automatiskt. När programmet avslutas sparas all data asynkront i JSON-format.

## 🔧 Funktioner
Användarhantering

Admin-knappen registrerar en ny Admin-användare.

Login-knappen autentiserar användaren baserat på dess roll (Admin/Customer).

## Datalagring & Återställning

- Produktlistan och användarlistan läses in vid programstart.

- All data sparas automatiskt vid programavslut.

## Produktadministration

- Produkter kan hanteras och filtreras efter produkttyp.

- Produkter kan ha bilder kopplade till sig.

## 🖥️ Exempel på funktionalitet
Vid programstart:

Om filerna inte finns, skapas nya filer.

- Produktlistan och användarlistan laddas från lagrade JSON-filer.

Vid programavslut:

- Alla ändringar sparas asynkront i JSON-filer.

## 🚀 Teknologier
- C#
- WPF (Windows Presentation Foundation)
- JSON för datalagring
- Asynkron filhantering
