import { route } from 'preact-router';
import { useState, useEffect } from "preact/hooks";

const Delete = ({ entityTypeName }) => {
    const [viewData, setViewData] = useState();
    const [keyValues, setKeyValues] = useState([]);
    const [errorMessage, setErrorMessage] = useState();

    useEffect(() => setKeyValues(new URL(document.location).searchParams.getAll('keys')), []);

    useEffect(() => {
        keyValues.length && (async () => {
            await fetch(
                `/Admin/api/form/entity/delete?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
                { credentials: 'include' })
                .then(r => r.json())
                .then(json => setViewData(json));
        })();
    }, [keyValues]);

    const deleteClick = async () => {
        setErrorMessage(null);
        await fetch(
            `/Admin/api/form/entity/delete?entityTypeName=${entityTypeName}&keys=${keyValues.join('&keys=')}`,
            { credentials: 'include', method: 'DELETE' })
            .then(r => r.json())
            .then(r => r.redirectUrl ? route(r.redirectUrl) : setErrorMessage(r.errorMessage));
    };

    return viewData && <>
        <div class="container">
            <h1 class="h2 mb-3">
                Delete {viewData.pluralLowerCaseName}&nbsp;
                <a class="btn btn-sm btn-beta" href={`/Admin/List/${entityTypeName}`}>Back</a>
            </h1>
        </div>
        <div class="container">
            <div class="card">
                <div class="card-body">
                    <div class="alert alert-primary" role="alert">
                        <p>You are going to delete the following {viewData.pluralLowerCaseName}:</p>
                        <ul>
                            <li>
                                <span>{viewData.instanceName} </span>
                                (with:
                                {Object
                                    .keys(viewData.primaryKeysWithValues)
                                    .map((key, index) => <>{!!index && ','} {key} <code>{viewData.primaryKeysWithValues[key]}</code></>)
                                }
                                )
                            </li>
                        </ul>
                        <p class="mb-0">This action is <strong>permanent</strong>. There is no going back.</p>
                    </div>

                    <button class="btn btn-primary" onClick={deleteClick}>Delete permanently</button>&nbsp;
                    <a class="btn btn-beta" href={`/Admin/List/${entityTypeName}`}>Back</a>
                    
                    {errorMessage && <div class="alert alert-danger mt-3" role="alert">{errorMessage}</div>}
                </div>
            </div>
        </div>
    </>;
};

export default Delete;