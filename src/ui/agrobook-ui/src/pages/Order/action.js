import { AxiosError } from "axios";
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

        if (response.status === 400) {
            return {
                Status: Status.Failed,
                Data: response.data.message
            }
        }

        return {
            Status: Status.Ok,
            Data: response.data
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
        if (orderDate instanceof Date) {
            let day = String(orderDate.getDate()).padStart(2, '0'); // dd
            let month = String(orderDate.getMonth() + 1).padStart(2, '0'); // MM (months are 0-indexed)
            let year = orderDate.getFullYear(); // yyyy
            orderDate = `${day}/${month}/${year}`;
        }
        else {
            orderDate = undefined
        }

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

        if (response.status === 204) {
            return {
                Status: Status.NoData,
                Data: undefined
            }
        }

        if (response.status === 200) {
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