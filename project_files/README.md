perdėliojimas drag-and-drop, su neutral zone kur gali pasidėt dėžutes.


Pakalbet apie deliojimo logika.
Kokiu dydziu gali but dezes, ar yra kazkokie standartai? visokiu
Kiek deziu per palete? ~1-30
Is kur zinot koki svori galima det ant dezes? 60kg
Kada galima bus matyti tikslu csv faila?

2650 aukstis (idet i options pakeist)
stackint nedaugiau 3 aukstais
iskart is jo atimti 200 (paletes dydis)

Plotis gali but 600,800,1200
Plotis pagal ilgi renkamas atitinkamai
1.8m > ilgis, plotis - 600
1.8m < ilgis, plotis - 800
3m > ilgis, plotis - 1200

2850x800 palete bus isrinkta.

2850,2050,200
1250,1400,115
1250,1400,115
1250,1400,115
1250,1400,115
1680,1780,115
952,1000,115
952,1000,115
852,852,115
852,852,115

Ima ilgiausia(pirmas skaic) krovini ir pagal ji nustato palete, - 2850 palete bus
ir pagal ilgi iteruoja kad sukist viska,
dar ziuri ka ant virsaus uzdet kad nevirsyti aukscio ir plocio.
ant daikto gali uzdet tik tokio pat plocio arba mazesnio, ir kad nevirsyt aukscio

Order turi: uzsakymo nr(int), kliento info (string), Alone (bool) (ar ant atskiros paletes deliojas, ar kartu su kitais)
Element: name, amount, width, height, depth, handle (future), weight, picture 
Accesorries: future




first iteration:
while possible to place something or items.Count > 0:
place largest possible item by length

second iteration:
while possible to place something or items.Count > 0:
place largest possible on top of first layer

third iteration:
while possible to place something or items.Count > 0:
place largest possible on top of second layer



Problems:
-- Drawings sheet some weird things are happening with sizing, sometimes goes out of bounds
-- Atpažinti "CWT" daiktus ir žinoti, kad virš jų dedasi tik tokio pat ilgio, arba išvis negali dėtis
-- Excel drawings sheet give images a margin, and bottom margin should be double, write amount of pieces below image


ADD:

DONE:
-- Atsiras ClientName, kurį reikės pridėt kur rašo Order Nr kaip string tsg. // DONE
-- LoadingDate atsiras failuose, ji turi prisidėti excelyje kaip žodis // DONE
-- Panaikinti laikinai BAR CODE // DONE
-- Pallet 1 Length, turėtų būt jau su pridėtu 50mm, tai keičia LDM, bet turi nesikeisti Pallet List // DONE
-- Check if stack weight works correctly // DONE
-- Add 5cm buffer to windows only if 2 or more are in the same row. // DONE
-- FIX: Delete config fuckery, just rewrite everything. // DONE
-- Add settings window with country selector.
-- Add loading animation, to know if the program has died.
-- Weird stacking behaviour, items get placed in wrong Y coordinate (f.e. first item gets placed in 600, then stacking continues, then Y cursor reaches 600 and it get's placed there again) // FIXED
-- After getting placed on pallets, some Items start their Y position not on 0 // FIXED
-- One Item now get's duplicated and an extra pallet is created // FIXED
-- Probably orderNr is broken on non-Alone pallets, since it just writes all orders as one // FIXED
-- One element get's lost in File Parsing // FIXED, coincadentally that was the glitch, which caused some files to not open
-- LDM cell formatting still off, fractal numbers get written in like 0.999999 instead of 1 // FIXED
-- When reading file client name was taken from list which can be empty. // FIXED
-- Stacking doesn't take into account weight // FIXED
-- Width Length excelyje sukeisti vietoms // DONE
-- Prie kiekvienos prekės kodo Pallet 1 sheete, turi būt skliaustuose užsakymo nr. (pvz: V10(1)) // DONE
-- When parsing Element data from file, individual items should remember their order, for reference. // DONE

