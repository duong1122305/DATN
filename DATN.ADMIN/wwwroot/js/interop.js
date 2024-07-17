window.uploadFileToGoogleDrive = async function (file) {
    const formData = new FormData();
    formData.append("file", file);

    try {
        const response = await fetch(
            "https://script.google.com/macros/s/AKfycbzZBdZh7safJaCFdWtIPOlE3N82KxqfBn-ez1B0wHOXk3QXwBgCj5aWBjUkeXRqAH6d/exec",
            {
                method: "POST",
                body: formData
            }
        );

        if (!response.ok) {
            throw new Error("Failed to upload file to Google Drive");
        }

        const data = await response.json();
        return data.link; // Assuming the response has a 'link' property
    } catch (error) {
        console.error("Error uploading file to Google Drive:", error);
        throw error;
    }
};

window.sendFileUrlToBackend = async function (fileUrl) {
    const userId = localStorage.getItem("idUser");
    const imageData = {
        userId: userId,
        urlImage: fileUrl
    };

    try {
        const response = await fetch(
            `https://localhost:7248/api/UploadFile/saveDb?UserId=${userId}&urlFile=${fileUrl}`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(imageData)
            }
        );

        return response.ok;
    } catch (error) {
        console.error("Error sending file URL to backend:", error);
        throw error;
    }
};
