import React from 'react';

export const InputError = props => {
    return (
        props.errors.map((e, i) => <p key={i} className="help is-danger">{e}</p>)
    );
}
