import React from 'react';
import { Form } from 'react-bulma-components';

const { Field, Control, Label, Textarea } = Form;

export const DescriptionInput = props => {
    return (
        <Field>
            <Label>Description</Label>
            <Control>
                <Textarea
                    name="description"
                    value={props.description}
                    onChange={props.handleChange} />
            </Control>
        </Field>);
}
