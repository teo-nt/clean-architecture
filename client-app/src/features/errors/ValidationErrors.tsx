import { Message } from "semantic-ui-react";

interface Props {
    errors: string[]
}

function ValidationErrors({ errors }: Props) {
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: string, index) => (
                        <Message.Item key={index}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
  );
}

export default ValidationErrors;