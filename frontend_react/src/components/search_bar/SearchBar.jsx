import { useState } from "react";

import { Icon } from '@iconify/react';
import baselineSearch from '@iconify/icons-ic/baseline-search';

import './search_bar.css'

export default function SearchBar({ onSubmit })
{
    // state to store the search string in.
    const [searchText, setSearchText] = useState('');

    // handle pressing the enter key to submit.
    const handleKeyPress = (event) => 
    {
        if(event.key === 'Enter')
        {
          submit();
        }
    }

    // this function calls the onSubmit property if its a function.
    const submit = () =>
    {
        // ensure the onSubmit property is a function.
        if (onSubmit instanceof Function)
        {
            // call onSubmit with the search text.
            onSubmit(searchText);
        }
    }

    // HTML for the search bar.
    return (
        <div className='search-bar-wrapper'>
            <input 
                className='search-bar-input' 
                type='text' 
                onChange={(event) => setSearchText(event.target.value)} 
                onKeyDown={handleKeyPress}>
            </input>

            <button onClick={submit} className='search-bar-button'><Icon icon={baselineSearch} width="20" height="20"/></button>
        </div>
    );
}