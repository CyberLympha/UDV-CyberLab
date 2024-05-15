// InstructionStep.tsx
import React, { useState, useEffect } from "react";
import { LabWork } from "../../../../api";
import {apiService} from "../../../services";
import style from "./InstructionStep.module.scss"

interface InstructionStepProps {
  labWork: LabWork;
  stepNumber: number;
}

export const InstructionStep: React.FC<InstructionStepProps> = ({ labWork, stepNumber }) => {
  const [stepText, setStepText] = useState<string>("");

  useEffect(() => {
    const fetchStepInstruction = async () => {
      const instruction: string | Error = await apiService.getStepInstruction(labWork.instructionId, stepNumber.toString());
      if (instruction instanceof Error)
        return
      setStepText(instruction);
    };

    fetchStepInstruction();
  }, [labWork, stepNumber]);

  return <div className={style.stepText}>{stepText}</div>;
};
