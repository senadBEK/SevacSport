# SevacSport

SevacSport je web informacioni sistem za prodaju sportske opreme, razvijen kao seminarski rad iz oblasti informacionih sistema. Aplikacija omogućava pregled kategorija i proizvoda, administraciju artikala, upravljanje korisnicima i osnovu za dalju nadogradnju sistema korpom i poručivanjem.

## Opis projekta

Cilj projekta je izrada višeslojne web aplikacije koja omogućava organizovan prikaz i upravljanje sportskom opremom. Sistem je namenjen prikazu proizvoda iz različitih sportskih kategorija, kao što su fudbal, košarka, odbojka, rukomet, fitness i trening oprema.

Aplikacija je realizovana korišćenjem ASP.NET Core MVC arhitekture, Entity Framework Core tehnologije i SQL Server baze podataka.

## Funkcionalnosti

- pregled kategorija sportske opreme
- pregled proizvoda
- dodavanje novih kategorija i proizvoda
- izmena postojećih podataka
- brisanje kategorija i proizvoda
- prikaz detalja proizvoda
- autentifikacija i autorizacija korisnika
- administrativni deo sistema
- osnova za proširenje sistema korpom i poručivanjem

## Tehnologije

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- HTML
- CSS
- Bootstrap

## Arhitektura sistema

Projekat je organizovan po višeslojnoj arhitekturi:

- **WebShop.MVC** – prezentacioni sloj, korisnički interfejs, kontroleri i view modeli
- **WebShop.BLL** – poslovna logika aplikacije
- **WebShop.DAL** – pristup podacima, modeli i rad sa bazom podataka

## Pokretanje projekta

1. Preuzeti ili klonirati projekat sa GitHub repozitorijuma.
2. Otvoriti fajl `WebShop.sln` u Visual Studio okruženju.
3. Proveriti connection string u fajlu `appsettings.json`.
4. Pokrenuti migracije nad bazom podataka pomoću Package Manager Console.
5. Pokrenuti aplikaciju iz projekta `WebShop.MVC`.

## Baza podataka

Aplikacija koristi SQL Server bazu podataka. Struktura baze zasniva se na entitetima kao što su:

- Category
- Product
- User
- Role

Proizvodi sadrže osnovne podatke kao što su šifra, naziv, opis, cena, brend, sport, veličina i pripadajuća kategorija.

## Moguća unapređenja

- potpuna implementacija korpe
- poručivanje proizvoda
- upload i prikaz slika proizvoda bez grešaka
- pretraga i filtriranje proizvoda
- unapređenje korisničkog interfejsa
- prikaz dostupnosti proizvoda na stanju

## Autor

Senad Bekirovski

## Napomena

Projekat je razvijen u edukativne svrhe kao seminarski rad iz oblasti informacionih sistema.