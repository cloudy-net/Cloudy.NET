import _ from "lodash";
import { useEffect, useState } from "preact/hooks";

export default ({ controlName, controlId, type, initialValue, initialBackendValue }) => {
    const [value, setValue] = useState(initialValue);
    const [backendValue, setBackendValue] = useState(initialBackendValue);

    const formatDate = valueAsNumber => {
        const date = new Date(valueAsNumber);
        const dateString = date.toISOString();

        if(type == 'date'){
            return dateString.split('T')[0];
        }

        if(type == 'time'){
            return dateString.split('T')[1].replace('Z', '');
        }

        const pad = value => value < 10 ? `0${value}` : value;

        const offset = date.getTimezoneOffset() * -1;
        const offsetHours = pad(Math.floor(offset / 60).toString());
        const offsetMinutes = pad((offset % 60).toString());
        const sign = offset < 0 ? '-' : '+';
        const formattedTimezoneOffset = `${sign}${offsetHours}:${offsetMinutes}`;
        const formattedDate = dateString.replace('Z', formattedTimezoneOffset);

        return formattedDate;
    };

    return <>
        <input type="hidden" name={controlName} value={backendValue} />
        <input type={type} class="form-control" id={controlId} value={value} onChange={event => { setValue(event.target.value); setBackendValue(formatDate(event.target.valueAsNumber)); }} />
    </>
};