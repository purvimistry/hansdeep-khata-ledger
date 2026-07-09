let villageSelect;
let areaSelect;

let entityModalOptions = {};

document.addEventListener("DOMContentLoaded", function () {
    initializeVillageDropdown();
    initializeAreaDropdown();
    initializeAreaState();
    const selectedVillageId = villageSelect.getValue();

    if (selectedVillageId) {
        loadAreas(selectedVillageId);
    }

    villageSelect.on("change", function (value) {
        areaSelect.clear();
        areaSelect.clearOptions();
        $("#VillageId").valid(); 
        document.getElementById("areaHint").style.display = "none";
        if (!value) {
            areaSelect.disable();
            document.getElementById("btnAddArea").disabled = true;
            return;
        }

        document.getElementById("btnAddArea").disabled = false;
        loadAreas(value);

    });
    document.getElementById("btnAddVillage").addEventListener("click", function () {
            openEntityModal({
                title: "Add Village",
                subtitle: "Create a new village.",
                label: "Village Name",
                saveButton: "Save Village",
                endpoint: "/Customer/CreateVillage"
            });
        });
    document.getElementById("btnAddArea").addEventListener("click", function () {
            const villageId = villageSelect.getValue();
            if (!villageId) {
                document.getElementById("areaHint").style.display = "block";
                return;
            }
            document.getElementById("areaHint").style.display = "none";
            openEntityModal({
                title: "Add Area",
                subtitle: "Create a new area.",
                label: "Area Name",
                saveButton: "Save Area",
                endpoint: "/Customer/CreateArea",
                selectedVillageName: villageSelect.options[villageId].text,
                extraData: {
                    villageId: villageId
                }
            });
    });

    document.getElementById("entitySaveBtn").addEventListener("click", saveEntity);
    document.getElementById("entityCancelBtn").addEventListener("click", closeEntityModal);

    document.getElementById("entityModalInput").addEventListener("keydown", function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            saveEntity();
        }
    });
    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            closeEntityModal();
        }
    });
    document.getElementById("entityModal").addEventListener("click", function (e) {
        if (e.target === this) {
            closeEntityModal();
        }
    });

})

function initializeVillageDropdown() {
    villageSelect = new TomSelect("#VillageId", {
        maxItems: 1,
        persist: false,
        create: false,
        placeholder: "Search Village..."
    });
    if (!document.getElementById("VillageId").value) {
        villageSelect.clear(true);
    }
}
function initializeAreaDropdown() {
    areaSelect = new TomSelect("#AreaId", {
        maxItems: 1,
        persist: false,
        create: false,
        placeholder: "Search Area..."
    });
}
function initializeAreaState() {

    const villageId = villageSelect.getValue();

    if (!villageId) {
        areaSelect.disable();
        return;
    }

    areaSelect.enable();
    document.getElementById("btnAddArea").disabled = false;
    loadAreas(villageId);
}
function openEntityModal(options) {

    entityModalOptions = options;
    document.getElementById("entityModalTitle").textContent = options.title;
    document.getElementById("entityModalSubtitle").textContent = options.subtitle;
    document.getElementById("entityModalLabel").textContent = options.label;
    document.getElementById("entitySaveBtn").textContent = options.saveButton;
    const input = document.getElementById("entityModalInput");
    input.value = (options.defaultValue || "").trim();

    setTimeout(() => {
        input.focus();
        input.select();
    }, 100);
    hideEntityError();

    const villageContainer = document.getElementById("entityVillageContainer");
    if (options.selectedVillageName) {
        villageContainer.style.display = "block";
        document.getElementById("selectedVillageName").textContent = options.selectedVillageName;
    }
    else {
        villageContainer.style.display = "none";
    }
    document.getElementById("entityModal")
        .classList.add("show");

}
function closeEntityModal() {

    document.getElementById("entityModal").classList.remove("show");
    document.getElementById("entityModalInput").value = "";
    const button = document.getElementById("entitySaveBtn");
    button.disabled = false;
    button.classList.remove("loading");
    button.textContent = entityModalOptions.saveButton || "Save";
    hideEntityError();

}
function showEntityError(message) {

    const error = document.getElementById("entityModalError");
    error.textContent = message;
    error.style.display = "block";

}
function hideEntityError() {

    const error = document.getElementById("entityModalError");
    error.style.display = "none";
    error.textContent = "";

}

async function saveEntity() {

    const input = document.getElementById("entityModalInput");
    const name = input.value.trim();
    if (name === "") {
        showEntityError("Please enter a name");
        return;
    }

    const button = document.getElementById("entitySaveBtn");
    button.disabled = true;
    button.classList.add("loading");
    const originalText = button.textContent;
    button.textContent = "Saving...";

    try {

        const token = document.querySelector('input[name="__RequestVerificationToken"]' ).value;
        const body = new URLSearchParams({
            name: name,
            ...(entityModalOptions.extraData || {})
        });
        const response = await fetch(
            entityModalOptions.endpoint,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": token
                },
                body: body
            });
        let result;
        try {
            result = await response.json();
        }
        catch {
            showEntityError("Unexpected server response");
            return;
        }
        if (!response.ok || !result.success) {
            showEntityError(result.message);
            return;
        }
        
        if (entityModalOptions.endpoint.includes("Village")) {

            villageSelect.addOption({
                value: result.id,
                text: result.text
            });

            villageSelect.setValue(result.id);
            $("#VillageId").valid();
            loadAreas(result.id);
        }
        else {

            areaSelect.addOption({
                value: result.id,
                text: result.text
            });

            areaSelect.setValue(result.id);
        }

        closeEntityModal();

    }
    catch {
        showEntityError(
            "Something went wrong. Please try again."
        );

    }
    finally {
        button.disabled = false;
        button.classList.remove("loading");
        button.textContent = originalText;
    }

}

async function loadAreas(villageId) {

    try {
        const selectedAreaId = document.getElementById("AreaId").value;
        const response = await fetch(`/Customer/GetAreas?villageId=${villageId}`);
        if (!response.ok)
            return;

        const areas = await response.json();
        areaSelect.clearOptions();
        areaSelect.clear(true);
        areas.forEach(area => {
            areaSelect.addOption({
                value: area.id,
                text: area.text
            });
        });

        areaSelect.enable();
        if (selectedAreaId) {
            areaSelect.setValue(selectedAreaId, true);
        }
        areaSelect.refreshOptions(false);
        areaSelect.refreshItems();
    }
    catch (error) {

        console.error("Unable to load areas", error);

    }

}