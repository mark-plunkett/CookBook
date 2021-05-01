import { InputError } from 'components/InputError';
import React from 'react';
import { Form } from 'react-bulma-components';
import { Field as FinalField } from 'react-final-form';

const { Field, Control, Label, Input } = Form;

export const TitleInput = props => {
    return (
        <FinalField name="title">
            {({ meta }) => (
                <Field>
                    <Control>
                        <Label>Title</Label>
                        <Input
                            type="text"
                            name="title"
                            value={props.title}
                            onChange={props.handleChange}
                            className={meta.submitError ? "is-danger" : ""} />
                    </Control>
                    { meta.submitError && <InputError errors={meta.submitError} />}
                </Field>
            )}
        </FinalField>);
}

