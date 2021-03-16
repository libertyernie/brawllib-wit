# brawllib-wit

A collection of command-line interfaces to [BrawlLib](https://github.com/soopercool101/BrawlCrate) that can be helpful in Wii Virtual Console injection.

## nodeextract

Extracts a single TEX0 or TPL node from a file that BrawlLib can open, such as a U8 archive.
The node must be the only TEX0 or TPL node with that name in the file.

Usage: `tex0extract.exe archive_file node_name output_file.png`

## nodereplace

Replaces a single TEX0 or TPL node from a file that BrawlLib can open, such as a U8 archive, with the contents of another file.
The node must be the only TEX0 or TPL node with that name in the file.

Usage: `nodereplace.exe archive_file node_name replacement_file.png`

## openingimet

Finds the game's name (in the given language) in the opening.bnr or 000000000.app file, and prints it to the console.

Usage: `openingimet.exe 000000000.app [ja|en|de|fr|es|it|nl|7|8|ko]`

## openingimet

Decompressed and extracts one of the three .arc files from the 00000000.app.

Usage: `openingextract.exe 000000000.app [banner|icon|sound] output.arc`

## u8pack

Creates a U8 archive that includes all files in the current directory.

Usage: `u8pack.exe output.arc`

## u8unpack

Extracts all files and folders in a U8 archive to the current directory.

Usage: `u8unpack.exe input.arc`
