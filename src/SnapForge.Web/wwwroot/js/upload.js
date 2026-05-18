(() => {
  const dropzone = document.querySelector("[data-dropzone]");
  const input = document.querySelector("[data-file-input]");
  const fileName = document.querySelector("[data-file-name]");

  if (!(dropzone instanceof HTMLElement) || !(input instanceof HTMLInputElement) || !(fileName instanceof HTMLElement)) {
    return;
  }

  const setFileName = () => {
    const file = input.files?.[0];
    fileName.textContent = file?.name ?? "No file selected";
    dropzone.classList.toggle("has-file", Boolean(file));
  };

  input.addEventListener("change", setFileName);

  dropzone.addEventListener("dragenter", event => {
    event.preventDefault();
    dropzone.classList.add("is-dragging");
  });

  dropzone.addEventListener("dragover", event => {
    event.preventDefault();
  });

  dropzone.addEventListener("dragleave", event => {
    if (!(event.relatedTarget instanceof Node) || !dropzone.contains(event.relatedTarget)) {
      dropzone.classList.remove("is-dragging");
    }
  });

  dropzone.addEventListener("drop", event => {
    event.preventDefault();
    dropzone.classList.remove("is-dragging");

    const files = event.dataTransfer?.files;
    if (!files || files.length === 0) {
      return;
    }

    input.files = files;
    setFileName();
  });
})();
