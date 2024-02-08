// bool CzyWidziszGracza()
// {
//     jeśli gracz znajduje się w polu widzenia
//     {
//         jeśli gracza i przeciwnika nie dzieli otoczenie
//         {
//             zwróć prawdę
//         }
//     }
//     zwróć fałsz
// }


// Celuj()
// {
//     jeśli rozpoczęto czynność
//     {
//         jeśli gracz ma poprzednią broń
//         {
//             poprzedniaBroń.PrzestańCelować()
//         }
//         jeśli gracz ma obecną broń
//         {
//             obecnaBroń.ZacznijCelować()
//         }
//     }
//     jeśli zakończono czynność
//     {
//         jeśli gracz ma obecną broń
//         {
//             obecnaBroń.PrzestańCelować()
//         }
//     }
// }


// UstawAnimacjęPistoletu(bool czyMaPistolet, bool czyKuca)
// {
//     jeśli czyMaPistolet == true
//     {
//         jeśli czyKuca == true
//         {
//             UstawKucanieZPistoletem()
//         }
//         w przeciwnym razie
//         {
//             UstawChodzenieZPistoletem()
//         }
//     }
//     w przeciwnym razie
//     {
//         jeśli czyKuca == true
//         {
//             UstawKucanieBezPistoletu()
//         }
//         w przeciwnym razie
//         {
//             UstawChodzenieBezPistoletu()
//         }
//     }
// }


// DodajPowiadomienieDoKolejki(string powiadomienie)
// {
//     jeśli powiadomienie nie jest w kolejce
//     {
//         DodajPowiadomienie()
//         Powiadomienie()
//     }
// }

// Powiadomienie()
// {
//     jeśli kolejka powiadomień nie jest pusta && nie wyświetla się powiadomienie
//     {
//         WyświetlPowiadomienie()
        
//         jeśli kolejka powiadomień nie jest pusta
//         {
//             Powiadomienie()
//         }
//     }
// }



// ObliczAmunicję(int podniesionaAmunicja)
// {
//     int różnica = maksymalnaIlośćAmunicji - obecnaIlośćAmunicji

//     jeśli (podniesionaAmunicja < różnica)
//     {
//         PodnieśCałąAmunicję()
//     }
//     w przeciwnym razie jeśli (obecnaIlośćAmunicji + różnica > maksymalnaIlośćAmunicji)
//     {
//         różnica = podniesionaAmunicja + obecnaIlośćAmunicji - maksymalnaIlośćAmunicji
//         PodnieśTyleAmunicjiIleMożesz()
//     }
//     jeśli (amunicjaWMagazynku < 1 && całaAmunicja > 0)
//     {
//         Przeładuj()
//     }
// }

