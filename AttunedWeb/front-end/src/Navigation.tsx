import { FunctionComponent, useRef, useState } from "react";
import { useRouteQuery } from "./Queries/useRouteQuery.ts";
import { PlaylistDto } from "./dtos/Dtos.ts";

interface NavigationProps {
  selectedPlaylist: PlaylistDto | undefined;
  setSelectedPlaylist: (playlist: PlaylistDto | undefined) => void;
}

const mediaPlaylistNames = [
  "Music",
  "Films",
  "TV Programmes",
  "Podcasts",
  "Audiobooks",
];

export const Navigation: FunctionComponent<NavigationProps> = (props) => {
  const [isOpen, setIsOpen] = useState(false);

  const { data, isFetching } = useRouteQuery<PlaylistDto[]>({
    url: "playlist",
    refetchOnWindowFocus: false,
  });

  const mediaPlaylists = data?.filter(
    (x) => mediaPlaylistNames.indexOf(x.Name) !== -1,
  );

  const playlists = data?.filter(
    (x) => mediaPlaylistNames.indexOf(x.Name) === -1,
  );

  const classNames = [
    "flex flex-col border-r min-w-[16rem] max-sm:fixed z-20 h-full bg-background pb-4",
    !isOpen && "max-sm:hidden",
  ];

  const navRef = useRef<HTMLDivElement>(null);
  const openButtonRef = useRef<HTMLButtonElement>(null);

  const close = () => {
    setIsOpen(false);
    setTimeout(() => openButtonRef.current?.focus());
  };

  const open = () => {
    setIsOpen(true);
    setTimeout(() => navRef.current?.focus());
  };

  return (
    <>
      {!isOpen && (
        <button
          ref={openButtonRef}
          className="sm:hidden p-2 m-2 mr-0 self-start text-2xl"
          aria-label="open nav"
          onClick={open}
        >
          ☰
        </button>
      )}
      {isOpen && (
        <div
          className="fixed sm:hidden w-full h-full bg-black z-10 opacity-50"
          onClick={close}
        />
      )}
      <nav
        ref={navRef}
        tabIndex={0}
        className={classNames.filter(Boolean).join(" ")}
        onKeyDown={(e) => e.key === "Escape" && close()}
      >
        <div className="flex px-4 pt-4 pb-2 bg-background justify-between">
          <h2 className="text-2xl">Navigation</h2>
          <button
            className="sm:hidden text-2xl"
            aria-label="close nav"
            onClick={close}
          >
            ✕
          </button>
        </div>
        <div className="bg-background h-full px-4 overflow-y-auto overflow-x-hidden">
          <ul>
            <li aria-current={props.selectedPlaylist === undefined}>
              <h3 className="text-xl flex">
                <button
                  className={[
                    "flex-1 flex",
                    props.selectedPlaylist === undefined && "bg-selected-row",
                  ]
                    .filter(Boolean)
                    .join(" ")}
                  onClick={() => props.setSelectedPlaylist(undefined)}
                >
                  All tracks
                </button>
              </h3>
            </li>
            <li>
              <h3 className="text-xl">Media categories</h3>
              <ul className="pl-2">
                {isFetching && <span>Loading...</span>}
                {mediaPlaylists &&
                  mediaPlaylists.map((x) => (
                    <li key={x.Id}>
                      <button
                        aria-label={x.Name}
                        className="truncate"
                        onClick={() => props.setSelectedPlaylist(x)}
                      >
                        {x.Name}
                      </button>
                    </li>
                  ))}
              </ul>
            </li>
            <li>
              <h3 className="text-xl">Playlists</h3>
              <ul className="pl-2">
                {isFetching && <span>Loading...</span>}
                {playlists &&
                  playlists.map((x) => (
                    <li
                      aria-current={x.Id === props.selectedPlaylist?.Id}
                      className="flex"
                      key={x.Id}
                    >
                      <button
                        className={[
                          "truncate flex flex-1",
                          x.Id === props.selectedPlaylist?.Id &&
                            "bg-selected-row",
                        ]
                          .filter(Boolean)
                          .join(" ")}
                        onClick={() => props.setSelectedPlaylist(x)}
                      >
                        <span
                          aria-label={x.IsSmart ? "Smart playlist" : "playlist"}
                          className="inline-block w-[2rem]"
                        >
                          {x.IsSmart ? "⛭" : "♫"}
                        </span>
                        {x.Name}
                      </button>
                    </li>
                  ))}
              </ul>
            </li>
          </ul>
        </div>
      </nav>
    </>
  );
};
