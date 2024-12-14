import { useState } from "react";
import { http } from "../../../http.ts";
import { UploadResponse } from "../../../api.ts";
import toast from "react-hot-toast";

function Upload() {
    const [objectsWithImage, setObjectsWithImage] = useState<UploadResponse[]>([]);
    const [image, setImage] = useState<File | null>(null);

    const handleUpload = async () => {
        if (!image) {
            toast.error("No image selected!");
            return;
        }

        const formData = new FormData();
        formData.append('file', image!);

        toast.promise(
            http.uploadCreate(formData),
            {
                loading: "Uploading file...",
                success: "File uploaded successfully!",
                error: "Error uploading file. Please try again."
            }
        ).then((response) => {
            const data = response.data;
            setObjectsWithImage((prevObjects) => [
                ...prevObjects,
                data
            ]);
        }).catch((error) => {
            console.error('Error during upload:', error);
        });
    };

    return (
        <>
            <input
                onChange={e => setImage(e.target.files![0]!)}
                type="file"
            />

            <button onClick={handleUpload}>Upload image</button>

            {objectsWithImage.map((object, index) => (
                <div key={index}>
                    <img src={object.url!} alt={object.title!} />
                    <p>{object.title}</p>
                </div>
            ))}
        </>
    );
}

export default Upload;
