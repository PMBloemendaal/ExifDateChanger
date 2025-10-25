# ExifDateChanger

## Translated by Google Gemini

## Objective of ExifDateChanger
For the creation of stereoscopic images, the XReal Beam PRO camera is utilized.
![XReal Beam PRO](https://eu.shop.xreal.com/cdn/shop/files/1.png?v=1718638918 "XReal Beam PRO")
The standard **firmware** of the XReal Beam PRO camera generates an **EXIF (Exchangeable Image File Format)** metadata set that is significantly incomplete. Specifically, there is a lack in the critical **DateTimeOriginal** metadata tag, which specifies the exact time of capture. Although this time information is consistently included in the filenames of the generated images, it is absent from the EXIF structure. This program has been developed to provide a solution by extracting the timestamp from the filename and **programmatically inserting** it into the EXIF metadata of the corresponding image file.

***

## Functional Specification
**ExifDateChanger** is a software utility based on the **Windows** operating system, designed for the **analysis and modification** of EXIF metadata within digital image files. After initializing and loading a file, the existing EXIF metadata is extracted and **displayed** to the user.

The primary functionality of the application includes:
1. The **conditional addition** of the creation date/time to the EXIF metadata, if the **DateTimeOriginal** tag is absent.
2. The **batch processing** of all image files within a specified directory. During this automated process, only files where the creation date is missing from the EXIF metadata are **modified and overwritten**.
3. If the required date/time information is already **validly** present in the EXIF structure, the source file is left **unmodified**.

***

## User Interface (UI)
The application is equipped with a **streamlined interface** featuring three primary controls:

### Load Picture
This command initiates a standard file selection dialog box, allowing the user to load an image file. The selected image is presented in the **image display area**. Subsequently, the complete, extracted EXIF metadata is displayed in a text field, with the **raw tag names** being replaced by **contextualized, readable labels** for improved user accessibility.

### Save Date
After a file has been loaded via `<Load Picture>` and the EXIF metadata has been analyzed, this command enables the **injection** of the timestamp. This occurs **only under the condition that the capture date and time is missing** from the EXIF metadata. The date/time is then parsed from the filename and written to the **DateTimeOriginal** tag.

### Save Date Folder
This command offers the functionality for **automated folder processing**. The user selects a target folder, after which the application **indexes and scans** all compatible image files within this folder. The EXIF metadata is extracted and evaluated for each file. If the capture date is missing, the program extracts the date from the filename and **commits** it to the EXIF metadata. If the capture date is already present, the file remains **unchanged**.

***

## Technical Specification
### Supported Metadata Tags
The application is capable of **parsing** and **visualizing** the following EXIF metadata tags to the user:
* ImageDescription
* Make
* Model
* Orientation (1 = Horizontal (normal), 2 = Mirror horizontal, 3 = Rotate 180, 4 = Mirror vertical, 5 = Mirror horizontal and rotate 270 CW, 6 = Rotate 90 CW, 7 = Mirror horizontal and rotate 90 CW, 8 = Rotate 270 CW)
* XResolution
* YResolution
* Resolution Unit (1 = None, 2 = inches, 3 = cm)
* Software
* Modifydate
* Thumbnaildata
* DateTimeOriginal
* ExposureTime
* FNumber
* Exposureprogram (0 = Not Defined, 1 = Manual, 2 = Program AE, 3 = Aperture-priority AE, 4 = Shutter speed priority AE, 5 = Creative (Slow speed), 6 = Action (High speed), 7 = Portrait, 8 = Landscape, 9 = Bulb)
* ISO
* Exifversion
* CreateDate
* MaxApertureValue
* OffsetDate
* OffsetDateOriginal
* BrightnessValue
* FocalLength
* Flash
* MeteringMode
* ShutterSpeedValue
* ApertureValue
* ExposureBiasValue
* SubSecTimeOriginal
* SubSecTimeDigitized
* ColorSpace (1 = None, 2 = inches, 3 = cm, 4 = mm, 5 = um)
* ImageWidth
ImageHeight
* ExposureMode (0 = Auto, 1 = Manual, 2 = Auto bracket)

### Date Metadata Tags
The following EXIF metadata tag names are searched for and optionally added if they are missing based on a filename:
* DateTimeOriginal
* DateTimeDigitized
* DateTime

### Filename Structure
The following filenames are parsed and converted into a date:
* xxxyyyymmdd_hhmmss for example SV_20250103_105519.jpg **XReal Beam PRO format**
* yyyy-mm-dd hh.mm.ss for example 2025-09-21 08.22.01.jpg
* xxx WhatsApp Image yyyy-mm-dd at hh.mm.ss.jpg

<hr>

# ExifDateChanger

## Doelstelling van ExifDateChanger
Voor het make van stereoscopische beelden wordt gebruikgemaakt van de XReal Beam PRO camera.
![XReal Beam PRO](https://eu.shop.xreal.com/cdn/shop/files/1.png?v=1718638918 "XReal Beam PRO")
De standaard **firmware** van de XReal Beam PRO camera genereert een **EXIF (Exchangeable Image File Format)** metadata-set die significant incompleet is. Er is met name een lacune in de kritieke **DateTimeOriginal** metadata tag, die de exacte tijd van de opname specificeert. Hoewel deze tijdsinformatie consistent is opgenomen in de bestandsnamen van de gegenereerde beelden, ontbreekt deze in de EXIF-structuur. Dit programma is ontwikkeld om een oplossing te bieden door de tijdstempel te extraheren uit de bestandsnaam en deze **programmatisch in te voegen** in de EXIF-metadata van het corresponderende afbeeldingsbestand.

***

## Functionele Specificatie
**ExifDateChanger** is een op **Windows** gebaseerd operating systeem, ontworpen voor de **analyse en modificatie** van EXIF-metadata binnen digitale afbeeldingsbestanden. Na het initialiseren en inlezen van een bestand, wordt de aanwezige EXIF-metadata geëxtraheerd en **getoond** voor de gebruiker.

De primaire functionaliteit van de applicatie omvat:
1.  De **conditionele toevoeging** van de creatiedatum/tijd aan de EXIF-metadata, indien deze **DateTimeOriginal** tag afwezig is.
2.  De **batchverwerking** van alle afbeeldingsbestanden binnen een gespecificeerde directory. Gedurende dit geautomatiseerde proces worden uitsluitend bestanden waarbij de creatiedatum ontbreekt in de EXIF-metadata **aangepast en overschreven**.
3.  Indien de vereiste datum-/tijdinformatie reeds **valide** aanwezig is in de EXIF-structuur, wordt het bronbestand **ongewijzigd** gelaten.

***

## Gebruikersinterface (UI)
De applicatie is voorzien van een **gestroomlijnde interface** met drie primaire bedieningselementen:

### Load Picture
Dit commando initieert een standaard dialoogvenster voor bestandselectie, waardoor de gebruiker een afbeeldingsbestand kan laden. Het geselecteerde beeld wordt in de **beeldweergavezone** (image display area) gepresenteerd. Aansluitend wordt de volledige, geëxtraheerde EXIF-metadata getoond in een tekstueel veld, waarbij de **ruwe tagnamen** worden vervangen door **gecontextualiseerde, leesbare labels** voor een betere gebruikerstoegankelijkheid.

### Save Date
Nadat een bestand is geladen via `<Load Picture>` en de EXIF-metadata is geanalyseerd, maakt dit commando de **injectie** van de tijdstempel mogelijk. Dit gebeurt **uitsluitend onder de conditie dat de datum en tijd van opname ontbreekt** in de EXIF-metadata. De datum/tijd wordt vervolgens geparseerd uit de bestandsnaam en weggeschreven naar de **DateTimeOriginal** tag.

### Save Date Folder
Dit commando biedt de functionaliteit voor **geautomatiseerde mapverwerking**. De gebruiker selecteert een doelmap waarna de applicatie alle compatibele afbeeldingsbestanden in deze map **indexeert en scant**. De EXIF-metadata wordt voor elk bestand geëxtraheerd en geëvalueerd. Indien de vastleggingsdatum ontbreekt, wordt de datum uit de bestandsnaam gehaald en aan de EXIF-metadata **gecommitteerd**. Wanneer de vastleggingsdatum al aanwezig is, blijft het bestand **onveranderd**.

***

## Technische Specificatie
### Ondersteunde Metadata Tags
De applicatie is in staat om de volgende EXIF-metadata tags te **parseren** en aan de gebruiker te **visualiseren**:# ExifDateChanger

## Doel van ExifDateChanger
Voor het maken van stereofoto's gebruik ik sinds deze camera:
![XReal Beam PRO](https://eu.shop.xreal.com/cdn/shop/files/1.png?v=1718638918 "XReal Beam PRO")
De standaard camera software van de XReal Beam PRO schrijft in de EXIF metadata vrijwel geen informatie. Een van de metadata tags die ik mis, is het tijdstip waarop de foto gemaakt is. Dit tijdstip staat wel in de naam van de afbeeldingen, maar dus niet in de EXIF data. Dit programma is gemaakt om de bestandsnaam van de afbeeldingen te gebruiken en aan de metadata van de afbeeldingen toe te voegen.

## Functionele specificatie
ExifDateChanger is een simpel programma gebaseerd op Windows om EXIF meta data van afbeeldingen in te lezen. De meeste EXIF metadata wordt gelezen en getoond na het laden van het programma.
Daarnaast bied dit programma de mogelijkheid om als de datum waarop een afbeelding gemaakt is niet aanwezig is in de EXIF metadata deze toe te voegen.
Ook heeft deze applicatie de mogelijkheid om dit proces binnen de afbeeldingen van 1 folder te automatiseren. Hierbij worden alle afbeeldingen gescand en alleen afbeeldingen waar bij creatie datum binnen de EXIF metadata ontbreekt toegevoegd. De originele afbeeldingen worden hierbij overschreven. 
Als de datum wel aanwezig is, wordt het bestand van de afbeelding ongewijzigd gelaten.

## Gebruiks interface
Het gebruik en werking is eenvoudig. Er zijn 3 knoppen aanwezig.

### Load Picture
Hier kan de gebruiker een afbeelding via een dialoogbox selecteren en laden. De afbeelding wordt getoond in het plaatjesveld. Daaron wordt in tekst alle EXIF metadata getoond die de afbeelding bevat, waarbij de meeste tagnames voor de gebruiker wordt vertaald.

### Save Date
Als een afbeelding via <Load Picture> ingeladen is en de EXIF metadata getoond wordt, kan * * alleen als de datum waarop de afbeelding gemaakt is ontbreekt* * , de datum uit de bestandsnaam worden gehaald en in de EXIF metadata worden geschreven.

### Save Date Folder
De gebruiker krijgt de mogelijkheid een folder te selecteren waarin afbeeldingen zijn opgenomen. Het programma zal hierna alle afbeeldingen scannen en de EXIF metadata extraheren. Indien de datum waarop een afbeelding is gemaakt ontbreekt, voegt het programma deze aan de EXIF metadata toe. Als de datum waarop de afbeelding is gemaakt al aanwezig is, wordt het bestand ongewijzigd gelaten.

## Technische specificatie
### Ondersteunde metadata tages
De volgende metadata tags worden herkend en getoond aan de gebruiker

* ImageDescription
* Make
* Model
* Orientation (1 = Horizontal (normal), 2 = Mirror horizontal, 3 = Rotate 180, 4 = Mirror vertical, 5 = Mirror horizontal and rotate 270 CW, 6 = Rotate 90 CW, 7 = Mirror horizontal and rotate 90 CW, 8 = Rotate 270 CW)
* XResolution
* YResolution
* Resolution Unit (1 = None, 2 = inches, 3 = cm)
* Software
* Modifydate
* Thumbnaildata
* DateTimeOriginal
* ExposureTime
* FNumber
* Exposureprogram (0 = Not Defined, 1 = Manual, 2 = Program AE, 3 = Aperture-priority AE, 4 = Shutter speed priority AE, 5 = Creative (Slow speed), 6 = Action (High speed), 7 = Portrait, 8 = Landscape, 9 = Bulb)
* ISO
* Exifversion
* CreateDate
* MaxApertureValue
* OffsetDate
* OffsetDateOriginal
* BrightnessValue
* FocalLength
* Flash
* MeteringMode
* ShutterSpeedValue
* ApertureValue
* ExposureBiasValue
* SubSecTimeOriginal
* SubSecTimeDigitized
* ColorSpace (1 = None, 2 = inches, 3 = cm, 4 = mm, 5 = um)
* ImageWidth
ImageHeight
* ExposureMode (0 = Auto, 1 = Manual, 2 = Auto bracket)

### Datum metadata tags
De volgende EXIF metadata tagnames worden gezocht en eventueel toe gevoegd als ze ontbreken in een bestandsdnaam:
* DateTimeOriginal
* DateTimeDigitized
* DateTime

### Bestandsnaam structuur
De volgende bestandsnamen worden geparsed en omgezet in een datum
* xxxyyyymmdd_hhmmss bijvoorbeeld SV_20250103_105519.jpg **XReal Beam PRO formaat**
* yyyy-mm-dd hh.mm.ss bijvoorbeeld 2025-09-21 08.22.01.jpg
* xxx WhatsApp Image yyyy-mm-dd at hh.mm.ss.jpg
