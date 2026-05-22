(() => {
  const panel = document.querySelector("[data-upload-panel]");
  const dropzone = document.querySelector("[data-dropzone]");
  const input = document.querySelector("[data-file-input]");
  const fileName = document.querySelector("[data-file-name]");

  if (
    !(panel instanceof HTMLElement)
    || !(dropzone instanceof HTMLElement)
    || !(input instanceof HTMLInputElement)
    || !(fileName instanceof HTMLElement)
  ) {
    return;
  }

  const isImageFile = file => file.type.startsWith("image/");

  const setFileName = () => {
    const file = input.files?.[0];
    fileName.textContent = file?.name ?? fileName.dataset.emptyText ?? "No file selected";
    dropzone.classList.toggle("has-file", Boolean(file));
  };

  const setDragging = isDragging => {
    panel.classList.toggle("is-dragging", isDragging);
    dropzone.classList.toggle("is-dragging", isDragging);
  };

  const showInvalidFile = () => {
    fileName.textContent = dropzone.dataset.invalidText ?? "Drop or paste an image file.";
    dropzone.classList.add("is-invalid");

    window.setTimeout(() => {
      dropzone.classList.remove("is-invalid");
      setFileName();
    }, 2200);
  };

  const assignFile = file => {
    if (!isImageFile(file)) {
      showInvalidFile();
      return;
    }

    const files = new DataTransfer();
    files.items.add(file);
    input.files = files.files;
    dropzone.classList.remove("is-invalid");
    setFileName();
  };

  const firstImageFile = files => {
    return Array.from(files ?? []).find(isImageFile);
  };

  input.addEventListener("change", setFileName);

  window.addEventListener("dragover", event => {
    if (event.dataTransfer?.types.includes("Files")) {
      event.preventDefault();
    }
  });

  window.addEventListener("drop", event => {
    if (event.dataTransfer?.types.includes("Files")) {
      event.preventDefault();
    }
  });

  panel.addEventListener("dragenter", event => {
    if (!event.dataTransfer?.types.includes("Files")) {
      return;
    }

    event.preventDefault();
    setDragging(true);
  });

  panel.addEventListener("dragover", event => {
    if (!event.dataTransfer?.types.includes("Files")) {
      return;
    }

    event.preventDefault();
    event.dataTransfer.dropEffect = "copy";
    setDragging(true);
  });

  panel.addEventListener("dragleave", event => {
    if (!(event.relatedTarget instanceof Node) || !panel.contains(event.relatedTarget)) {
      setDragging(false);
    }
  });

  panel.addEventListener("drop", event => {
    event.preventDefault();
    setDragging(false);

    const file = firstImageFile(event.dataTransfer?.files);
    if (!file) {
      showInvalidFile();
      return;
    }

    assignFile(file);
  });

  document.addEventListener("paste", event => {
    const activeElement = document.activeElement;
    const isTyping =
      activeElement instanceof HTMLInputElement
      || activeElement instanceof HTMLTextAreaElement
      || activeElement instanceof HTMLSelectElement
      || activeElement?.getAttribute("contenteditable") === "true";

    if (isTyping) {
      return;
    }

    const file = firstImageFile(event.clipboardData?.files);
    if (!file) {
      return;
    }

    event.preventDefault();
    assignFile(file);
  });
})();
