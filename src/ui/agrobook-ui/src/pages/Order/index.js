import { useEffect, useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import { postFileAsync, Status, generatePdf, getPreOrders } from "./action"
import SingleAlertComponent from "../../components/SingleAlertComponent"
import PreOrderTable from "./PreOrderTableComponenet";

function Order() {

    const [pageData, setPageData] = useState({
        fileToUpload: undefined,
        progressCount: 0,
        fileUploading: false,
        isDowloadingPdf: false,
        currentPdfDate: undefined,
        alertMessage: { success: true, message: '', timeout: undefined },
        preOrders: [],
    })

    useEffect(() => {
        const setPreOrders = async () => {
            let preOrders = await getPreOrders();

            if (preOrders.Status == Status.Ok)
            {
                setPageData(p => ({
                    ...p,
                    preOrders: preOrders.Result
                }))
            }
        }

        Promise.all([setPreOrders()]);
    },[])

    const handleFileChange = (event) => {
        setPageData(p => ({
            ...p,
            fileToUpload: event.target.files[0]
        }))
    };

    const handleUpload = async () => {
        if (!pageData.fileToUpload) {
            setPageData(p => ({
                ...p,
                alertMessage: { success: false, message: `Adicione um arquivo primeiro.` },
            }));
            return;
        }

        setPageData(p => ({
            ...p,
            isUploading: true,
            alertMessage: 'Uploading',
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

            if (result.Status == Status.Ok) {
                let r =
                    Array.isArray(result.Data)
                        ? result.Data.length
                        : 0;

                setPageData(p => ({
                    ...p,
                    alertMessage: { success: true, message: `Completo com sucesso. ${r} novos pedidos feitos.` },
                }));
                return;
            }

            let r = result.Data;
            setPageData(p => ({
                ...p,
                alertMessage: {
                    success: false,
                    message: r ? `Falha no processamento. ${r}.` : 'Falha no processamento.'
                },
            }));
        } catch (error) {
            console.error('Error uploading file:', error);
            setPageData(p => ({
                ...p,
                alertMessage: { success: false, message: `Falha no processamento. Verifique com o administrador.` },
            }));
        } finally {
            setPageData(p => ({
                ...p,
                isUploading: false
            }));
        }
    };

    const handlePdfGeneration = async () => {
        try {

            setPageData(p => ({
                ...p,
                isDowloadingPdf: true
            }))

            let pdfResult = await generatePdf(pageData.currentPdfDate)

            if (pdfResult.Status == Status.NoData) {
                setPageData(p => ({
                    alertMessage: { success: true, message: "Nenhum pedido em aberto.", timeout: 5000 }
                }))
            }

            if (!(pdfResult.Data instanceof Blob)) {
                return;
            }

            if (pageData.currentPdfDate instanceof Date)
                downloadBlob(pdfResult.Data, `Pedidos-${pageData.currentPdfDate.toISOString().split('T')[0]}.pdf`);
            else
                downloadBlob(pdfResult.Data, `Pedidos.pdf`);

        } finally {

            setPageData(p => ({
                ...p,
                isDowloadingPdf: false
            }))
        }
    }

    const downloadBlob = (blob, filename) => {
        // Create a temporary URL for the blob
        const url = URL.createObjectURL(blob);

        const a = document.createElement('a');
        a.href = url;
        a.download = filename || 'download';
        a.target = "_blank"

        document.body.appendChild(a);
        a.click();

        setTimeout(() => {
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    };

    const handleOrderDateChange = (e) => {
        let v = e.target.value;

        let date = new Date(v);

        setPageData(p => ({
            ...p,
            currentPdfDate: date
        }))
    }

    return (
        <>
            <PageNavigationBar />

            <SingleAlertComponent
                message={pageData.alertMessage.message}
                kind={pageData.alertMessage.success ? 'success' : 'danger'}
                timeout={pageData.alertMessage.timeout} />

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
                        <div class="d-grid gap-2 d-flex">
                            <button type="button" onClick={handleUpload} disabled={pageData.isUploading} class="btn btn-primary btn-lg">
                                <span id="submitText">Fazer Upload</span>
                                <span id="spinner" class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                            </button>

                        </div>
                    </form>

                    <div className="mt-3">
                        <label for="ordersDateInput">Data pedido</label>
                        <input id="ordersDateInput" type="date" class="form-control" placeholder="Selecione a data que os pedidos serão entregues." onChange={handleOrderDateChange}/>
                        <button type="button" onClick={handlePdfGeneration} disabled={pageData.isDowloadingPdf} class="btn btn-secondary btn-lg mt-1">
                            <span id="submitText">Fazer download do PDF</span>
                            <span id="spinner" class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                        </button>
                    </div>

                    <PreOrderTable data={pageData.preOrders} />
                </div>
            </div>

        </>
    );
}

export default Order;