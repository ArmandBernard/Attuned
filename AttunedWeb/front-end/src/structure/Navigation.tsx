import { FunctionComponent, RefObject, useRef, useState } from "react";
import { useRouteQuery } from "@utils/useRouteQuery.ts";
import { PlaylistDto } from "@dtos";
import { Link, useMatchRoute, useParams } from "@tanstack/react-router";
import { Dialog } from "@components/Dialog.tsx";

const mediaPlaylistNames = [
  "Music",
  "Films",
  "TV Programmes",
  "Podcasts",
  "Audiobooks",
];

export const Navigation: FunctionComponent = () => {
  const [isOpen, setIsOpen] = useState(false);

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
      <button
        ref={openButtonRef}
        className="sm:hidden fixed bg-background rounded-full p-2 m-2 mr-0 self-start text-2xl"
        aria-label="open nav"
        onClick={open}
      >
        ☰
      </button>
      <Dialog
        className="ml-0 mt-0 text-text-color"
        show={isOpen}
        closeOnBackgroundClick
        onClose={close}
      >
        <Contents
          isOpen={isOpen}
          navRef={navRef}
          onClose={() => setIsOpen(false)}
        />
      </Dialog>
      <Contents
        isOpen={true}
        className="max-sm:hidden"
        navRef={navRef}
        onClose={() => setIsOpen(false)}
      />
    </>
  );
};

function Contents({
  isOpen,
  onClose,
  navRef,
  className,
}: {
  isOpen: boolean;
  onClose: () => void;
  navRef: RefObject<HTMLElement>;
  className?: string;
}) {
  const params = useParams({ strict: false });

  const matchRoute = useMatchRoute();

  const isAlbums = !!matchRoute({ to: "/albums" });
  const isRoot = !!matchRoute({ to: "/" });

  const playlistId: number | undefined = params.playlistId
    ? parseInt(params.playlistId)
    : undefined;

  const { data, isFetching } = useRouteQuery<PlaylistDto[]>({
    url: "playlist",
    refetchOnWindowFocus: false,
  });

  const selectedPlaylist =
    playlistId !== undefined
      ? data?.find((playlist) => playlist.Id === playlistId)
      : undefined;

  const mediaPlaylists = data?.filter(
    (x) => mediaPlaylistNames.indexOf(x.Name) !== -1,
  );

  const playlists = data?.filter(
    (x) => mediaPlaylistNames.indexOf(x.Name) === -1,
  );

  const navClassNames = [
    "fixed flex flex-col border-r w-80 z-20 h-full bg-background pb-4",
    !isOpen && "hidden",
    className,
  ].filter(Boolean);

  return (
    <nav
      ref={navRef}
      tabIndex={0}
      className={navClassNames.join(" ")}
      onKeyDown={(e) => e.key === "Escape" && close()}
    >
      <div className="flex px-4 pt-4 pb-2 bg-background justify-between">
        <h2 className="text-2xl">Navigation</h2>
        <button
          className="sm:hidden text-2xl"
          aria-label="close nav"
          onClick={onClose}
        >
          ✕
        </button>
      </div>
      <div className="bg-background h-full px-4 overflow-y-auto overflow-x-hidden">
        <ul>
          <li aria-current={isRoot}>
            <h3 className="text-xl flex">
              <Link
                className={["flex-1 flex", isRoot && "bg-selected-row"]
                  .filter(Boolean)
                  .join(" ")}
                to="/"
              >
                All tracks
              </Link>
            </h3>
          </li>
          <li aria-current={isAlbums}>
            <h3 className="text-xl flex">
              <Link
                className={["flex-1 flex", isAlbums && "bg-selected-row"]
                  .filter(Boolean)
                  .join(" ")}
                to="/albums"
              >
                Albums
              </Link>
            </h3>
          </li>
          <li>
            <h3 className="text-xl">Media categories</h3>
            <ul className="pl-2">
              {isFetching && <span>Loading...</span>}
              {mediaPlaylists &&
                mediaPlaylists.map((x) => (
                  <li key={x.Id}>
                    <Link
                      aria-label={x.Name}
                      className="truncate"
                      to="/playlists/$playlistId"
                      params={{ playlistId: x.Id.toString() }}
                    >
                      {x.Name}
                    </Link>
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
                    aria-current={x.Id === selectedPlaylist?.Id}
                    className="flex"
                    key={x.Id}
                  >
                    <Link
                      className={[
                        "truncate flex flex-1",
                        x.Id === selectedPlaylist?.Id && "bg-selected-row",
                      ]
                        .filter(Boolean)
                        .join(" ")}
                      to="/playlists/$playlistId"
                      params={{ playlistId: x.Id.toString() }}
                    >
                      <span
                        aria-label={x.IsSmart ? "Smart playlist" : "playlist"}
                        className="inline-block w-[2rem] flex-shrink-0"
                      >
                        {x.IsSmart ? "⛭" : "♫"}
                      </span>
                      <span className="truncate">{x.Name}</span>
                    </Link>
                  </li>
                ))}
            </ul>
          </li>
        </ul>
      </div>
    </nav>
  );
}
