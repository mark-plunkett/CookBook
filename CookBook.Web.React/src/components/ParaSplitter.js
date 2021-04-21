import React from 'react';

export const ParaSplitter = props => {
    return (props.string.split('\n').map((p, idx) => <p key={idx}>{p}</p>));
}