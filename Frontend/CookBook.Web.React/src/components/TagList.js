import React from 'react';
import { Element, Tag } from 'react-bulma-components';

export const TagList = props => {
    return (
        <Element className="has-text-right">
            {props.tags.map(t => {
                const tag = props.allTags[t];
                return <Tag className="ml-1" key={tag.canonicalized}>{tag.name}</Tag>;
            })}
        </Element>
    );
}
