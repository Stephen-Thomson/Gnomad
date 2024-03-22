// internal imports.
import { useEffect, useState } from "react";
import "./list_element.css";

export default function ListElement({
    pin,
    onClick
})
{
    const [active, setActive] = useState(false);

    const clicked = () =>
    {
        setActive(!active);
    }

    useEffect(()=>
    {
        onClick(pin, active);
    }, [active]);

    if (active)
    {
        return (
            <li onClick={clicked} className='list-element element-active'>{ pin.title }</li>
        );
    }
    else
    {
        return (
            <li onClick={clicked} className='list-element'>{ pin.title }</li>
        );
    }
    
}