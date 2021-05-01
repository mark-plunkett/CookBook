import React from 'react';
import { Form } from 'react-bulma-components';

const { Field, Control, Label, Input } = Form;

export const NumberOfServingsInput = props => {
    return (
        <Field>
            <Control>
                <Label>Number of servings</Label>
                <Input
                    type="number"
                    min="1"
                    name="servings"
                    value={props.servings}
                    onChange={props.handleChange} />
            </Control>
        </Field>);
}
