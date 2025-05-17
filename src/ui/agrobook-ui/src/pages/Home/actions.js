import api from '../../api/AzFunApi'

export const getProducts = async () => {
    let response = await api.get('api/product')

    if (response.status === 401){
        return {
            Status: Status.Unauthorized,
            Result: "Não autorizado"
        }
    }

    if (response.status !== 200 || 
        response.headers['Content-Type'] !== "application/json" ||
        !Array.isArray(response.data)){
        return {
            Status: Status.Error,
            Result: "Falha ao coletar dados."
        }
    }

    return {
        Status: Status.Ok,
        Result: response.data.map(x => {
            x.
        })
    }
}

export const Status = {
    Unauthorized: "Unauthorized",
    Error: "Error",
    Ok: "Ok"
}