import { useEffect, useState } from "react";
import PageNavigationBar from "../../components/PageNavigationBar";
import { postFileAsync, Status, generatePdf, getPreOrders, generatePdfV2, generatePdfByFile } from "./action";
import SingleAlertComponent from "../../components/SingleAlertComponent";
import PreOrderTable from "./PreOrderTableComponenet";
import PdfGenerationModal from "../../components/PdfGenerationModal"

function Order() {
    const [pageData, setPageData] = useState({
        fileToUpload: undefined,
        progressCount: 0,
        fileUploading: false,
        isDownloadingPdf: false,
        currentPdfDate: new Date(),
        alertMessage: { success: true, message: '', timeout: undefined, show: false },
        preOrders: [],
        showPdfModal: false,
        csvFileForPdf: undefined
    });

    // Clear alert after timeout
    useEffect(() => {
        if (pageData.alertMessage.timeout && pageData.alertMessage.show) {
            const timer = setTimeout(() => {
                setPageData(p => ({
                    ...p,
                    alertMessage: { ...p.alertMessage, show: false }
                }));
            }, pageData.alertMessage.timeout);

            return () => clearTimeout(timer);
        }
    }, [pageData.alertMessage.timeout, pageData.alertMessage.show]);

    useEffect(() => {
        const setPreOrders = async () => {
            try {
                let preOrders = await getPreOrders();
                if (preOrders.Status === Status.Ok) {
                    setPageData(p => ({
                        ...p,
                        preOrders: preOrders.Result
                    }));
                } else {
                    showAlert(false, "Falha ao carregar pedidos prévios.");
                }
            } catch (error) {
                console.error('Error loading pre-orders:', error);
                showAlert(false, "Erro ao carregar pedidos prévios.");
            }
        };

        setPreOrders();
    }, []);

    const showAlert = (success, message, timeout = 5000) => {
        setPageData(p => ({
            ...p,
            alertMessage: { success, message, timeout, show: true }
        }));
    };

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        if (file && !file.name.toLowerCase().endsWith('.csv')) {
            showAlert(false, "Por favor, selecione um arquivo CSV.");
            return;
        }

        setPageData(p => ({
            ...p,
            fileToUpload: file
        }));
    };

    function checkFirstRowForComma(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();

            reader.onload = function (e) {
                const content = e.target.result;
                const firstLine = content.split('\n')[0];
                const hasComma = firstLine.includes(',');
                resolve(hasComma);
            };

            reader.onerror = function () {
                reject(new Error('Failed to read file'));
            };

            reader.readAsText(file);
        });
    }

    const handleUpload = async () => {
        if (!pageData.fileToUpload) {
            showAlert(false, "Adicione um arquivo primeiro.");
            return;
        }

        setPageData(p => ({
            ...p,
            fileUploading: true,
            progressCount: 0
        }));

        try {
            let filePreferences = (await checkFirstRowForComma(pageData.fileToUpload))
                ? 'en-US'
                : 'pt-BR';
                
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
                },
                filePreferences
            );
            
            if (result.Status === Status.Ok) {
                let newOrdersCount = Array.isArray(result.Data) ? result.Data.length : 0;
                showAlert(true, `Completo com sucesso. ${newOrdersCount} novos pedidos feitos.`);

                // Refresh pre-orders list
                const preOrders = await getPreOrders();
                if (preOrders.Status === Status.Ok) {
                    setPageData(p => ({
                        ...p,
                        preOrders: preOrders.Result
                    }));
                }

                // Clear file input
                setPageData(p => ({
                    ...p,
                    fileToUpload: undefined
                }));

                // Reset file input
                const fileInput = document.querySelector('input[type="file"]');
                if (fileInput) fileInput.value = '';

                return;
            }

            let errorMessage = result.Data ? `Falha no processamento. ${result.Data}.` : 'Falha no processamento.';
            showAlert(false, errorMessage);
        } catch (error) {
            console.error('Error uploading file:', error);
            showAlert(false, "Falha no processamento. Verifique com o administrador.");
        } finally {
            setPageData(p => ({
                ...p,
                fileUploading: false,
                progressCount: 0
            }));
        }
    };

    const handlePdfGeneration = async (version) => {
        try {
            setPageData(p => ({
                ...p,
                isDownloadingPdf: true
            }));

            let pdfResult = version === "v2"
                ? await generatePdfV2(pageData.currentPdfDate)
                : await generatePdf(pageData.currentPdfDate);

            if (pdfResult.Status === Status.NoData) {
                showAlert(true, "Nenhum pedido em aberto.", 5000);
                return;
            }

            if (!(pdfResult.Data instanceof Blob)) {
                showAlert(false, "Erro ao gerar PDF.");
                return;
            }

            const dateString = pageData.currentPdfDate instanceof Date
                ? pageData.currentPdfDate.toISOString().split('T')[0]
                : 'pedidos';

            const versionSuffix = version === "v2" ? "-v2" : "";
            downloadBlob(pdfResult.Data, `Pedidos-${dateString}${versionSuffix}.pdf`);

            showAlert(true, "PDF gerado com sucesso!");

        } catch (error) {
            console.error('Error generating PDF:', error);
            showAlert(false, "Erro ao gerar PDF.");
        } finally {
            setPageData(p => ({
                ...p,
                isDownloadingPdf: false
            }));
        }
    };

    const handlePdfByFileGeneration = async () => {
        try {
            setPageData(p => ({
                ...p,
                isDownloadingPdf: true
            }));

            let pdfResult = await generatePdfByFile(pageData.currentPdfDate, pageData.csvFileForPdf);

            if (pdfResult.Status === Status.NoData) {
                showAlert(true, "Nenhum pedido em aberto.", 5000);
                return;
            }

            if (!(pdfResult.Data instanceof Blob)) {
                showAlert(false, "Erro ao gerar PDF.");
                return;
            }

            const dateString = pageData.currentPdfDate instanceof Date
                ? pageData.currentPdfDate.toISOString().split('T')[0]
                : 'pedidos';

            downloadBlob(pdfResult.Data, `Pedidos-${dateString}.pdf`);

            showAlert(true, "PDF gerado com sucesso!");

        } catch (error) {
            console.error('Error generating PDF:', error);
            showAlert(false, "Erro ao gerar PDF.");
        } finally {
            setPageData(p => ({
                ...p,
                isDownloadingPdf: false
            }));
        }
    };

    const downloadBlob = (blob, filename) => {
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        a.target = "_blank";

        document.body.appendChild(a);
        a.click();

        setTimeout(() => {
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        }, 100);
    };

    const handleOrderDateChange = (e) => {
        const value = e.target.value;
        const date = new Date(value);

        setPageData(p => ({
            ...p,
            currentPdfDate: date
        }));
    };

    const handlePdfModalToggle = () => {
        setPageData(p => ({
            ...p,
            showPdfModal: !p.showPdfModal,
            csvFileForPdf: undefined
        }));
    };

    const handleCsvFileChange = (event) => {
        const file = event.target.files[0];
        if (file && !file.name.toLowerCase().endsWith('.csv')) {
            showAlert(false, "Por favor, selecione um arquivo CSV.");
            return;
        }

        console.log("File set")
        setPageData(p => ({
            ...p,
            csvFileForPdf: file
        }));
    };

    const isUploadButtonDisabled = pageData.fileUploading || !pageData.fileToUpload;
    const isPdfButtonDisabled = pageData.isDownloadingPdf;

    return (
        <>
            <PageNavigationBar />

            {pageData.alertMessage.show && (
                <SingleAlertComponent
                    message={pageData.alertMessage.message}
                    kind={pageData.alertMessage.success ? 'success' : 'danger'}
                    timeout={pageData.alertMessage.timeout}
                />
            )}

            <div className="container-sm">
                <div className="upload-container bg-white p-4 rounded shadow-sm">
                    {/* Upload Section */}
                    <div className="text-center mb-4">
                        <div className="upload-icon text-primary mb-3">
                            <svg xmlns="http://www.w3.org/2000/svg" width="3em" height="3em" fill="currentColor" viewBox="0 0 16 16" className="bi bi-cloud-arrow-up">
                                <path fillRule="evenodd" d="M7.646 5.146a.5.5 0 0 1 .708 0l2 2a.5.5 0 0 1-.708.708L8.5 6.707V10.5a.5.5 0 0 1-1 0V6.707L6.354 7.854a.5.5 0 1 1-.708-.708l2-2z" />
                                <path d="M4.406 3.342A5.53 5.53 0 0 1 8 2c2.69 0 4.923 2 5.166 4.579C14.758 6.804 16 8.137 16 9.773 16 11.569 14.502 13 12.687 13H3.781C1.708 13 0 11.366 0 9.318c0-1.763 1.266-3.223 2.942-3.593.143-.863.698-1.723 1.464-2.383zm.653.757c-.757.653-1.153 1.44-1.153 2.056v.448l-.445.049C2.064 6.805 1 7.952 1 9.318 1 10.785 2.23 12 3.781 12h8.906C13.98 12 15 10.988 15 9.773c0-1.216-1.02-2.228-2.313-2.228h-.5v-.5C12.188 4.825 10.328 3 8 3a4.53 4.53 0 0 0-2.941 1.1z" />
                            </svg>
                        </div>
                        <p className="text-muted mb-4">Selecione o arquivo de pedidos CSV para realizar o upload</p>
                    </div>

                    <form id="uploadForm" encType="multipart/form-data">
                        <div className="mb-3">
                            <label htmlFor="fileInput" className="form-label">Escolha o arquivo CSV</label>
                            <input
                                className="form-control"
                                type="file"
                                id="fileInput"
                                accept=".csv"
                                required
                                onChange={handleFileChange}
                                disabled={pageData.fileUploading}
                            />
                        </div>

                        {/* Progress Bar */}
                        {pageData.fileUploading && (
                            <div className="mb-3">
                                <div className="progress">
                                    <div
                                        className="progress-bar"
                                        role="progressbar"
                                        style={{ width: `${pageData.progressCount}%` }}
                                        aria-valuenow={pageData.progressCount}
                                        aria-valuemin="0"
                                        aria-valuemax="100"
                                    >
                                        {pageData.progressCount}%
                                    </div>
                                </div>
                            </div>
                        )}

                        <div className="d-grid gap-2 d-md-flex justify-content-md-start">
                            <button
                                type="button"
                                onClick={handleUpload}
                                disabled={isUploadButtonDisabled}
                                className="btn btn-primary btn-lg"
                            >
                                {pageData.fileUploading ? (
                                    <>
                                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                        Enviando...
                                    </>
                                ) : (
                                    'Fazer Upload'
                                )}
                            </button>
                        </div>
                    </form>

                    {/* PDF Generation Section */}
                    <div className="mt-5 pt-4 border-top">
                        <div className="mb-3">
                            <label htmlFor="ordersDateInput" className="form-label">Data entrada dos pedidos</label>
                            <input
                                id="ordersDateInput"
                                type="date"
                                className="form-control"
                                value={pageData.currentPdfDate.toISOString().split('T')[0]}
                                onChange={handleOrderDateChange}
                            />
                        </div>

                        <div className="btn-group" role="group" aria-label="Selecione a versão do PDF">
                            <button
                                type="button"
                                onClick={() => handlePdfGeneration()}
                                disabled={isPdfButtonDisabled}
                                className="btn btn-secondary btn-lg"
                            >
                                {pageData.isDownloadingPdf ? (
                                    <>
                                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                        Gerando...
                                    </>
                                ) : (
                                    'Fazer download do PDF'
                                )}
                            </button>
                            <button
                                type="button"
                                onClick={() => handlePdfGeneration("v2")}
                                disabled={isPdfButtonDisabled}
                                className="btn btn-secondary btn-lg"
                            >
                                {pageData.isDownloadingPdf ? (
                                    <>
                                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                                        Gerando...
                                    </>
                                ) : (
                                    'Fazer download do PDF V2'
                                )}
                            </button>
                        </div>

                        {/* PDF Generation from CSV Modal Trigger */}
                        <div className="mt-3">
                            <button
                                type="button"
                                className="btn btn-outline-primary"
                                onClick={handlePdfModalToggle}
                            >
                                Gerar PDF a partir de pedidos via CSV
                            </button>
                        </div>
                    </div>

                    {/* Pre-Orders Table */}
                    <div className="mt-5">
                        <PreOrderTable data={pageData.preOrders} />
                    </div>

                    <PdfGenerationModal
                        show={pageData.showPdfModal}
                        onHide={handlePdfModalToggle}
                        csvFile={pageData.csvFileForPdf}
                        onCsvFileChange={handleCsvFileChange}
                        onGeneratePdf={handlePdfByFileGeneration}
                    />
                </div>
            </div>
        </>
    );
}

export default Order;