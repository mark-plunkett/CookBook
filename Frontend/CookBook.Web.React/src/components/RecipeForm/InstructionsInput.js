import React from 'react';
import { Form } from 'react-bulma-components';

const { Field, Control, Label, Textarea } = Form;

export const InstructionsInput = props => {
    return (
        <Field>
            <Label>Instructions</Label>
            <Control>
                <Textarea
                    name="instructions"
                    value={props.instructions}
                    onChange={props.handleChange} />
            </Control>
        </Field>);
}
