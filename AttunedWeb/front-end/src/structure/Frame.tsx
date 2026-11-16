import { Outlet } from "@tanstack/react-router";
import { Navigation } from "@root/structure/Navigation.tsx";
import IconButton from "@components/IconButton.tsx";
import SettingsIcon from "@components/icons/SettingsIcon.tsx";
import { useState } from "react";
import { SettingsDialog } from "@views/SettingsDialog.tsx";

export function Frame() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="flex h-full">
      <div className="sm:w-80 max-sm:w-8">
        <Navigation />
      </div>
      <Outlet />
      <div className="absolute top-2 right-2">
        <IconButton
          aria-label="Settings"
          title="Settings"
          onClick={() => setIsOpen(true)}
        >
          <SettingsIcon />
        </IconButton>
      </div>
      <SettingsDialog isOpen={isOpen} onClose={() => setIsOpen(false)} />
    </div>
  );
}
