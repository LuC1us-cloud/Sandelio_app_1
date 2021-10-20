# Sandelio_app_1

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
1.8m > plotis, aukstis - 600
1.8m < plotis, aukstis - 800
3m > plotis - 1200

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


