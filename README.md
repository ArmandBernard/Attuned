# Attuned

A chill, personal project attempting to create an iTunes replacement.
In particular, I wanted something with the same smart playlist functionality that could sync with my Android phone.

Currently not even in alpha yet.

Huge kudos to [cvzi](https://github.com/cvzi)'s [itunes_smartplaylist](https://github.com/cvzi/itunes_smartplaylist)
project, which I based the parsing code on.

## Main (planned) features

- [x] Decode XML into an in-memory data structure.
- [ ] Create a GUI to show this data.
- [ ] Generate non-smart playlist files for Android.

## Not planned potential features

- Media Player
- Enable syncing files to Android.
- Write code to allow generation of XML file.
- Enable editing of track data via the GUI.
- Enable editing of smart playlists via the GUI

## Tech stack

My first attempt at this project was with .NET Framework 4 and WinForms. Now I'm targeting .NET Core and planning a
WebAPI + React front-end.