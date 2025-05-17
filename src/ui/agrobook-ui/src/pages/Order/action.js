import api from "../../api/AzFunApi"

export const postFileAsync = async (file, uploadProgress = (progressEvent) => { }) => {
    try {

        const formData = new FormData();
        formData.append('file', file);

        const response = await api.post(
            'api/financial/order/batch',
            formData,
            {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                onUploadProgress: uploadProgress
            });

        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        const result = await response.json();
        
        return {
            Status: Status.Ok,
            Data: result
        };
    } catch (err) {
        console.error('Failed to generate orders.', err)
        
        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const generatePdf = async (orderDate) => {
    try {
        const response = await api.post(
            'api/financial/order/pdf',
            {
                orderDate: orderDate
            },
            {
                headers: {
                    'Content-Type': 'application/json'
                },
                responseType: 'blob'
            });

        if (response.status === 401) {
            return {
                Status: Status.Unauthorized,
                Data: undefined
            };
        }

        if (response.status === 204){
            return {
                Status: Status.NoData,
                Data: undefined
            }
        }

        if (response.headers.contentType === 'application/pdf') {
            const blob = new Blob([response.data], { type: 'application/pdf' });
            return {
                Status: Status.Ok,
                Data: blob
            };
        }

        throw new Error('Unexpected response type: ' + response.headers.contentType);
    } catch (err) {
        console.error('Failed to generate PDF.', err)
        return {
            Status: Status.Failed,
            Data: "Falha ao processar."
        };
    }
}

export const Status = {
    Unauthorized: 'Não autorizado',
    Ok: 'Ok',
    NoData: 'Nenhum dado encontrado',
    Failed: 'Falha'
};