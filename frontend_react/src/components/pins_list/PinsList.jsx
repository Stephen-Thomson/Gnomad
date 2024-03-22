import { useEffect, useState } from "react";
import ListElement from "./ListElement";
import Pin from "../../data/pin";
import "./pins_list.css";

export default function PinsList({
    pins,
    onClick
})
{

    const clicked = (pin, active) =>
    {
        onClick(pin, active);
    }

    let list_items = pins.map(
        (item) => 
        <ListElement pin={item} key={item.id} onClick={clicked}></ListElement>
    );

    /*
    const addPin = (pin) =>
    {
        setPins([
            ...pins,
            pin
        ]);
    }

    const removePin = (pin) =>
    {
        setPins(
            pins.filter(p => p.id !== pin.id)
        );
    }
    */

    useEffect(() =>
    {
        let pin = new Pin(0, 0, 10, 20, 'Pin Title', 'Street', []);
        //setPins([pin, pin, pin, pin, pin, pin, pin]);
    }, []);

    return (
        <div className='multi-select'>
            <ul className='list'>
                { list_items }
            </ul>
        </div>
    );
}