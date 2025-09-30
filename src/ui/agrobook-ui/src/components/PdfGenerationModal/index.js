// PdfGenerationModal.jsx
function PdfGenerationModal({ show, onHide, csvFile, onCsvFileChange, onGeneratePdf }) {
    return (
        <div className={`modal fade ${show ? 'show d-block' : ''}`} tabIndex="-1" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Gerar PDF a partir de pedidos via CSV</h5>
                        <button type="button" className="btn-close" onClick={onHide}></button>
                    </div>
                    <div className="modal-body">
                        <div className="mb-3">
                            <label htmlFor="csvFileInput" className="form-label">Escolha o arquivo CSV</label>
                            <input
                                className="form-control"
                                type="file"
                                id="csvFileInput"
                                accept=".csv"
                                onChange={onCsvFileChange}
                            />
                        </div>
                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={onHide}>
                            Cancelar
                        </button>
                        <button 
                            type="button" 
                            className="btn btn-primary" 
                            onClick={onGeneratePdf}
                            disabled={!csvFile}
                        >
                            Gerar PDF
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default PdfGenerationModal;