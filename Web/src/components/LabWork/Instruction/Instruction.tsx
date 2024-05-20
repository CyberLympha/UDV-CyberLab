import React, { useState, useEffect } from "react";

import {useToast} from "@chakra-ui/react";
import {apiService} from "../../../services";
import {userStore} from "../../../stores";
import {LabWork} from "../../../../api";
import style from "./Instruction.module.scss"
import {InstructionStep} from "./InstructionStep";

interface InstructionProps {
  labWork: LabWork;
  stopLabWork: (userId: string | undefined) => Promise<void>;
}

export const Instruction: React.FC<InstructionProps>  = ({ labWork, stopLabWork }) => {
  const [currentStep, setCurrentStep] = useState(1);
  const [stepAmount, setStepAmount] = useState(0);
  const [hintText, setHintText] = useState<string>("");
  const [isHintVisible, setIsHintVisible] = useState<boolean>(false);
  const toast = useToast();

  useEffect(() => {
    const fetchStepAmount = async () => {
      const amount = await apiService.getInstructionStepAmount(labWork.instructionId);
      if (amount instanceof Error)
        return
      setStepAmount(amount);
    };

    fetchStepAmount();
  }, [labWork]);

  const handleNextStep = async () => {
    if (!userStore.user?.id){
      return
    }
    const isCorrect = await apiService.checkIfAnswerCorrect(userStore.user?.id, labWork.id, currentStep.toString());
    if (isCorrect) {
      toast({
        title: 'Пункт успешно выполнен',
        status: "success",
        duration: 5000,
        isClosable: true,
        position: "top"
      });
      if (currentStep === stepAmount){
        await handleFinish()
      }
      else{
        setCurrentStep(prevStep => prevStep + 1);
      }
    } else {
      toast({
        title: 'Пункт не выполнен',
        status: "error",
        duration: 5000,
        isClosable: true,
        position: "top"
      });
    }
  };

  const handleFinish = async () => {
    await stopLabWork(userStore.user?.id);
  };

  const getHint = async () => {
    const text = await apiService.getInstructionStepHint(labWork.instructionId, currentStep.toString());
    if (text instanceof Error)
      return
    setHintText(text);
    setIsHintVisible(true);
  };
  
  const closeHint = () => {
    setIsHintVisible(false);
  };

  return (
    <div className={style.instructionContainer}>
      <div className={style.instructionTitle}>Инструкция</div>
      <div className={style.instructionContent}>
        <div className={style.currentStep}>Шаг {currentStep}</div>
        <InstructionStep labWork={labWork} stepNumber={currentStep} />
        <button className={style.nextButton} onClick={handleNextStep}>{currentStep === stepAmount ? "Закончить" : "Дальше"}</button>
        <button className={style.hintButton} onClick={getHint}>Посмотреть подсказку</button>
      </div>

      {isHintVisible && (
        <div className={style.overlay} onClick={closeHint}>
          <div className={style.hintBox}>
            <div className={style.hintTitle}>Подсказка</div>
            <div className={style.hintText}>{hintText}</div>
            <button className={style.closeButton} onClick={closeHint}>Закрыть</button>
          </div>
        </div>
      )}

    </div>
  );
};
