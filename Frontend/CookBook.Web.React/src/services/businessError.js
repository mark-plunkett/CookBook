
export const mapErrorsToObject = errors => {
    return errors.reduce((errorMap, v) => {
        if (!errorMap[v.propertyName]) errorMap[v.propertyName] = [v.message];
        else errorMap[v.propertyName].push(v.message);
        return errorMap;
    }, {});
}