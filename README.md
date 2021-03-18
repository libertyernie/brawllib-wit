# brawllib-wit

A collection of command-line interfaces to [BrawlLib](https://github.com/soopercool101/BrawlCrate) that can be helpful in Wii Virtual Console injection.

## bannercfgfromtxt

banner.cfg.txt is a text file found inside misc.ccf in some Virtual Console games from Sega. It controls the game title used in save data.

This program reads the given banner.cfg.txt file and replaces each field with the contents of a text file in the current directory with the same name as the field.
For example, the Japanese name will be replaced with the contents of JP.txt, and the German name with what's in GE.txt (if it exists, otherwise the existing title is kept).

Usage: `bannercfgfromtxt.exe banner.cfg.txt`

## imgoverlay

Overlays one image onto another at a certain position and writes the output to a third file.
This can be useful when creating icons and banners for Wii Virtual Console save data.
It uses GDI+ and you might get better (or worse) results with another tool, but it's included here for the sake of completeness.

Usage: `imgoverlay.exe larger.png smaller.png x y width height output.png`

## openingimet

Finds the game's name (in the given language) in the opening.bnr or 000000000.app file, and prints it to standard output.
Languages can be referred to by number (0, 1, 2..) or ISO 639-1 code (ja, en, de...).

Usage: `openingimet.exe 000000000.app [ja|en|de|fr|es|it|nl|7|8|ko]`

## openingextract

Decompresses and extracts one of the three .arc files from the 00000000.app.

Usage: `openingextract.exe 000000000.app [banner|icon|sound] output.arc`

## texextract

Extracts a single TEX0 or TPL texture to an image file.

Usage: `texextract.exe input_file.tex0 output_file.png`

## texreplace

Replaces a single TEX0 or TPL texture with an image from another file.
In most cases, the standard BrawlCrate image import dialog will be shown.

Usage: `texreplace.exe file_to_modify.tex0 replacement_file.png`

## u8pack

Creates a U8 archive that includes all files and folders in the current directory.

Usage: `u8pack.exe output.arc`

## u8unpack

Extracts all files and folders in a U8 archive to the current directory.
Like most other apps in this collection, it uses BrawlLib to parse the archive, and it may be successful in cases where other tools are not (or vice versa).

Usage: `u8unpack.exe input.arc`

## wteconvert

.wte is an image format found inside misc.ccf in some Virtual Console games from Sega. It contains the icon animation frames and banner used in save data.

This program converts RGB5A3 data between the .wte format and TEX0v1, which is supported by BrawlLib.
It be used in conjunction with texextract and texreplace to convert to and from PNG.

Usage: `wteconvert.exe file.[wte|tex0] out.[wte|tex0]`
