import React from 'react';
import { Form } from 'react-bulma-components';

const { Field, Control, Label, Textarea } = Form;

export const IngredientsInput = props => {
    return (
        <Field>
            <Label>Ingredients</Label>
            <Control>
                <Textarea
                    name="ingredients"
                    value={props.ingredients}
                    onChange={props.handleChange} />
            </Control>
        </Field>);
}
