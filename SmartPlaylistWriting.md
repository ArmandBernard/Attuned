# Writing smart playlists: where we are

Notes for picking up the work on editing and creating smart playlists. The back-end writer is done and tested. The
controller, DTOs and front end are not started.

## What exists

All in `iTunesParser/`:

| Piece | What it does |
|---|---|
| `Playlists/CriteriaEncoder.cs` | `Conjunction` to the bytes of `Smart Criteria`. The inverse of `CriteriaParser`. **The byte layout is documented at the top of this file.** |
| `Playlists/InfoWriter.cs` | Sets values in the 112 bytes of `Smart Info` (limit, live updating, matching). The inverse of `InfoHelper`. |
| `Xml/PlaylistWriter.cs` | `ApplyPatch(doc, id, PlaylistPatch)` edits a playlist in the XML document in place. Follows `TrackListWriter`. |
| `Data/PlaylistPatch.cs` | Partial update: name, limit, live updating, rules. A `null` rules value turns matching off and keeps the saved rules. |
| `Xml/PlistDataFormatter.cs` | Writes base64 `<data>` text with the line length and indentation of the text it replaces. |
| `Xml/PlistDictWriter.cs` | Set or remove a key in a plist dict. Moved out of `TrackListWriter` so both writers share it. |

Parser fixes that were needed so the writer and parser agree:

- **Strings** are a byte length followed by big endian UTF-16. The parser used to read them with a one-byte shifted
  little endian read, which only works for Latin-1.
- **Dates** are an unsigned count of seconds since 1904. The parser used to treat them as milliseconds.

## What is proven

Proven by comparing with smart playlists iTunes wrote (the 52 in the old test library, and the "TEST" playlists made in
iTunes on 2026-11-28):

- **`Smart Info`:** generated from scratch it matches iTunes except for byte 21 (see below).
- **`Smart Criteria`:** generated from scratch it matches iTunes for every playlist, except that the order of rules and
  groups within one list can differ (see below).
- Rules that have been checked against iTunes output: strings (is, contains, starts with, ends with, and negated),
  ratings (range, not, greater than, less than), absolute dates (range and greater than), "in the last N
  days/weeks/months" (and negated), media kind and location lists, nesting to three levels, all/any.
- Editing keeps every `Smart Info` byte that is not understood, so an edit never loses anything iTunes put there.

## What is NOT proven

These are written from the layout that was worked out, but no iTunes output has been compared:

- **Numbers:** Plays, Skips, BPM, Bit Rate, Duration, Size, Year, Track Number, Disc Number, Compilation. Ratings are
  proven, other integer fields are assumed to be stored the same way.
- **Booleans:** Checked, Purchased, Has Artwork. Assumed to be an "is 1" number rule.
- **Lists:** Love, iCloud Status. (Media Kind and Location are proven.)
- **Dates:** "is" and "less than" (only "range" and "greater than" are proven).
- **Playlist rules** ("playlist is X"). The writer throws `NotSupportedException`. The parser reads the playlist's id
  from a different place to the one cvzi's later version reads, and there is no sample to say which is right.
- **Limits:** only the defaults (25 items, random) and "most played" have been compared. Other units and "selected by"
  options, "match only checked items", and "least" rather than "most" are written from the known byte offsets.
  `Smart Info` bytes 12 and 13 have never been seen set to anything but 0.
- **Top level "any".** The all/any byte is proven in groups, but every playlist seen has "all" at the top level.
- Non-Latin text in a rule is tested against hand-built bytes, not against iTunes.

### Open question: `Smart Info` byte 21

It is 0 in some playlists and 1 in others (17 of the 52 old ones). It does not follow the rules, sort order, limit,
live updating, number of tracks, or any other key saved with the playlist. Every playlist made fresh in iTunes (the
three "TEST Byte 21" playlists) has 0, so new playlists are generated with 0. Something that happens later, possibly
editing the playlist in iTunes, sets it to 1. Nothing is known about what it does.

### Known limitation: order of rules and groups

iTunes lets a rule come before or after a group in the same list. `Conjunction` keeps rules and groups in separate lists,
so it forgets the order. The old library had both orders. `CriteriaEncoder.Encode` takes the playlist's existing
criteria as an optional second argument, and uses it to keep the order of any list whose number of rules and groups has
not changed. A new playlist gets rules first. Fixing this properly means putting the order in the model, which also
affects the DTOs and front end.

## Tests

Run everything that does not need a private library:

    dotnet test iTunesParserTests/iTunesParserTests.csproj

(209 tests at the time of writing.)

| File | Covers |
|---|---|
| `Playlists/CriteriaEncoderTests.cs` | Bytes against what iTunes wrote, hand-built bytes, every rule type, ordering, rejected input |
| `Playlists/InfoWriterTests.cs` | Known bytes, unknown bytes are kept, from-scratch output against iTunes, validation |
| `Xml/PlaylistWriterTests.cs` | Patching, the document being unchanged for no-op patches and failures, layout of the data text |
| `Xml/TestPlaylists/*.xml` | Playlists saved by iTunes. The four `Test*.xml` were made on purpose, and have their track lists removed. |
| `Xml/TestPlaylists/SmartPlaylistTestData.cs` | Shared fixtures and helpers to build criteria by hand |

### Tests that need the private library

`Xml/SmartPlaylistRealLibraryTests.cs` (and the older real library tests) are `[Explicit]` and `LocalOnly`. They read a
real library, which is not in source control (`/*.xml` is ignored). The test project copies
`iTunes Music Library.xml` from the repo root to the output folder as `iTunesMusicLibrary.xml` when it exists. To run
them:

    dotnet test iTunesParserTests/iTunesParserTests.csproj --filter "Category=LocalOnly"

They check, for every smart playlist in the library: `Smart Info` re-encodes exactly, criteria generated from scratch
matches iTunes (except for order), criteria re-encoded with the order hint matches exactly, and applying a playlist's
own values with `PlaylistWriter` changes nothing in the saved file.

Note that `iTunesParserTests/iTunesMusicLibrary.xml` (the old copy, 52 smart playlists) was deleted, and a manual test
that used it needs it put back.

## Playlists still wanted from iTunes

Make smart playlists with these rules, then export the library (or save it, as the root `iTunes Music Library.xml` is
a copy of the live file) and compare. Name them starting with "TEST".

1. Numbers: Plays, Skips, BPM, Bit Rate, Duration, Size, Year, Track Number, Disc Number, Compilation.
2. Booleans: Checked, Purchased, Has Artwork (true and false).
3. Lists: Kind, Love, Cloud Status, and Location with "is not".
4. "Playlist is X" and "Playlist is not X". This is needed before playlist rules can be written.
5. Dates: Date Added, Last Played and Last Skipped with "is" and "is before".
6. Limits: every unit, every "selected by" option with both most and least variants, "match only checked items", live
   updating off, and a limit with rule matching off.
7. Top level "any", and a list with a group before a rule and another with a rule before a group.
8. A rule with Japanese text in it.
9. For byte 21: edit one of the fresh playlists in iTunes (change a rule and save, then change it back) and see
   whether the byte changes.

For each one, the quickest check is to add the playlist to the fixtures as an XML file in `Xml/TestPlaylists/` (a
playlist's `<dict>` up to the end of `Smart Criteria`) and list it in `SmartPlaylistTestData.SmartPlaylists`. That
gives it all of the from-scratch and unchanged-patch tests.

## Next steps

1. Compare the playlists above, fix whatever the encoder gets wrong, and move rules from "not proven" to "proven".
2. Back-end for the web API: a `PATCH /Playlist/{id}` that follows `TrackController.Patch` (`[RequireWritable]`, a
   locked read-modify-write in a `PlaylistWriter`-using class like `TrackWriter`, cache invalidation), a patch DTO that
   can read rules (the rule DTOs only write today), and DTO to model converters. `Dtos.ts` is regenerated by building
   `AttunedWeb`.
3. `Playlist Items` (the saved list of tracks) is not recomputed after the rules change. Decide whether to clear it,
   leave it for iTunes to rebuild, or write a rule evaluator.
4. Creating new smart playlists: the encoders are written for it (`InfoWriter.Encode(smartInfo)` and
   `CriteriaEncoder.Encode`), but adding the playlist element to the XML (ids, persistent id, key order) is not done.
5. Front end editing.
