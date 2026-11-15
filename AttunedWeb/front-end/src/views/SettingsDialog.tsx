import { Dialog } from "@components/Dialog.tsx";
import Toggle from "@components/Toggle.tsx";
import { useState } from "react";
import CloseIcon from "@components/icons/CloseIcon.tsx";
import IconButton from "@components/IconButton.tsx";

interface SettingsDialogProps {
  isOpen: boolean;
  onClose: () => void;
}

export function SettingsDialog({ isOpen, onClose }: SettingsDialogProps) {
  const [readonlyMode, setReadonlyMode] = useState<boolean>(true);

  return (
    <Dialog
      className="max-sm:w-full sm:max-w-[500px] sm:w-4/5 bg-background text-text-color m-auto rounded-sm"
      show={isOpen}
      onClose={onClose}
      closeOnBackgroundClick
    >
      <div className="min-h-32 p-4 flex flex-col gap-2">
        <h1 className="text-xl">Settings</h1>
        <IconButton
          aria-label="Close"
          title="Close"
          className="absolute top-2 right-2"
          onClick={onClose}
        >
          <CloseIcon />
        </IconButton>

        <h2 className="text-lg">Read-only mode</h2>
        <div className="text-sm bg-warning/25 p-2 rounded-sm">
          Turning off read-only mode will allow changes to be made to your
          iTunes library. This is still experimental.
        </div>
        <label className="flex items-center justify-between gap-2">
          Enable read-only mode{" "}
          <Toggle
            checked={readonlyMode}
            onChange={(checked) => setReadonlyMode(checked)}
          />
        </label>
      </div>
    </Dialog>
  );
}
