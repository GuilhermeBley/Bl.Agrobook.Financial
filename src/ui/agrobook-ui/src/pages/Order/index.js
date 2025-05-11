import { useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import { postFileAsync, Status } from "./action"

function Order() {

    const [pageData, setPageData] = useState({
        fileToUpload: undefined,
        progressCount: 0,
        fileUploading: false,
        uploadStatus: ''
    })

    const handleFileChange = (event) => {
        setPageData(p => ({
            ...p,
            fileToUpload: event.target.files[0]
        }))
    };

    const handleUpload = async () => {
        if (!selectedFile) {
            setUploadStatus('Please select a file first');
            return;
        }

        setPageData(p => ({
            ...p,
            isUploading: true,
            uploadStatus: 'Uploading',
            setUploadProgress: 0
        }));

        try {
            let result = await postFileAsync(
                pageData.fileToUpload,
                (progressEvent) => {
                    const progress = Math.round(
                        (progressEvent.loaded / progressEvent.total) * 100
                    );
                    setPageData(p => ({
                        ...p,
                        progressCount: progress,
                    }));
                });

            if (result.Status == Status.Ok)
            {
                let r = 
                    Array.isArray(result.Data)
                    ? result.Data.length
                    : 0;

                setPageData(p => ({
                    ...p,
                    uploadStatus: `Completo com sucesso. ${r} novos pedidos feitos.`,
                }));
                return;
            }

            let r = result.Data.Message;
            setPageData(p => ({
                ...p,
                uploadStatus: `Falha no processamento. Erro: ${r}.`,
            }));
        } catch (error) {
            console.error('Error uploading file:', error);
            setPageData(p => ({
                ...p,
                uploadStatus: `Falha no processamento.`,
            }));
        } finally {
            setPageData(p => ({
                ...p,
                isUploading: false
            }));
        }
    };

    return (
        <>
            <PageNavigationBar />

            <div class="container-sm">
                <div class="upload-container bg-white">
                    <div class="text-center">
                        <div class="upload-icon">
                            <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" fill="currentColor" viewBox="0 0 16 16" class="bi bi-cloud-arrow-up">
                                <path fill-rule="evenodd" d="M7.646 5.146a.5.5 0 0 1 .708 0l2 2a.5.5 0 0 1-.708.708L8.5 6.707V10.5a.5.5 0 0 1-1 0V6.707L6.354 7.854a.5.5 0 1 1-.708-.708l2-2z" />
                                <path d="M4.406 3.342A5.53 5.53 0 0 1 8 2c2.69 0 4.923 2 5.166 4.579C14.758 6.804 16 8.137 16 9.773 16 11.569 14.502 13 12.687 13H3.781C1.708 13 0 11.366 0 9.318c0-1.763 1.266-3.223 2.942-3.593.143-.863.698-1.723 1.464-2.383zm.653.757c-.757.653-1.153 1.44-1.153 2.056v.448l-.445.049C2.064 6.805 1 7.952 1 9.318 1 10.785 2.23 12 3.781 12h8.906C13.98 12 15 10.988 15 9.773c0-1.216-1.02-2.228-2.313-2.228h-.5v-.5C12.188 4.825 10.328 3 8 3a4.53 4.53 0 0 0-2.941 1.1z" />
                            </svg>
                        </div>
                        <p class="text-muted mb-4">Selecione o arquivo de pedidos CSV para realizar o upload</p>
                    </div>

                    <form id="uploadForm" enctype="multipart/form-data">
                        <div class="mb-3">
                            <label for="fileInput" class="form-label">Escolha o arquivo CSV</label>
                            <input
                                class="form-control"
                                type="file"
                                required
                                onChange={handleFileChange}
                                disabled={pageData.isUploading} />
                        </div>
                        <div class="d-grid gap-2">
                            <button type="button" onClick={handleUpload} disabled={pageData.isUploading} class="btn btn-primary btn-lg">
                                <span id="submitText">Fazer Upload</span>
                                <span id="spinner" class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

export default Order;