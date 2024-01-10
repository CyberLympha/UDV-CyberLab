import {useToast} from "@chakra-ui/react";
import Modal from 'react-bootstrap/Modal';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

import React, { useState } from 'react';
import { Button as LocalButton } from '../../Button/Button';
import { apiService } from '../../../services';
import {CreateLabReservationRequest, Lab } from '../../../../api';
import { userStore } from '../../../stores';
import style from './AddLabReservation.module.scss'

interface Props {
  show: boolean;
  handleClose: () => void;
  selectedLab: Lab | null;
  selectedWeek: Date;
  fetchScheduleData: (selectedWeek: Date, selectedLab: Lab | null) => void;
}

export function convertDateToNetTicks(date: Date): number {
  return date.getTime() * 10000 + 621355968 * 1000000000 - date.getTimezoneOffset() * 600000000
}

export const AddLabReservation: React.FC<Props> = ({
  show,
  handleClose,
  selectedLab,
  selectedWeek,
  fetchScheduleData,
}) => {
  const [timeStart, setTimeStart] = useState(new Date());
  const [timeEnd, setTimeEnd] = useState(new Date());
  const [theme, setTheme] = useState('');
  const [description, setDescription] = useState('');
  const toast = useToast();

  const handleSaveReservation = async () => {
    const newReservation: CreateLabReservationRequest = {
      timeStart: convertDateToNetTicks(timeStart),
      timeEnd: convertDateToNetTicks(timeEnd),
      theme,
      description,
      reservorId: userStore.user?.id,
      lab: selectedLab,
    };

    const response = await apiService.createLabReservation(newReservation);
    if (!(response instanceof Error)) {
      handleClose();
      fetchScheduleData(selectedWeek, selectedLab);
      toast({
        title: 'Резервация была успешно создана',
        status: "success",
        duration: 4000,
        isClosable: true,
        position: "top"
    })
    } else {
      toast({
        title: 'Ошибка при создании резервации',
        description: `${response}`,
        status: "error",
        duration: 4000,
        isClosable: true,
        position: "top",
        containerStyle: {zIndex: 9999},
    })
    }
  };

  return (
    <Modal className={style.addReservationOverlay} show={show} onHide={handleClose} dialogClassName={style.addReservationModal}>
      <Modal.Header closeButton>
        <Modal.Title>Добавить резервацию</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <form className={style.addReservationForm}>
        <div className={style.formGroup}>
              <label>Тема:</label>
              <input type="text" value={theme} onChange={(e) => setTheme(e.target.value)} />
            </div>
            <div className={style.formGroup}>
              <label>Описание:</label>
              <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
            </div>
            <div className={style.timeInputs}>
              <div className={style.formGroup}>
                <label>Время начала:</label>
                <DatePicker
            selected={timeStart}
            onChange={(date: Date) => setTimeStart(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
              <div className={style.formGroup}>
                <label>Время конца:</label>
                <DatePicker
            selected={timeEnd}
            onChange={(date: Date) => setTimeEnd(date)}
            showTimeSelect
            timeFormat="HH:mm"
            dateFormat="MMMM d, yyyy h:mm aa"
            className={style.datePicker}
          />
              </div>
            </div>
        </form>
      </Modal.Body>
      <Modal.Footer>
        <LocalButton variant="primary" onClick={handleSaveReservation}>
          Сохранить
        </LocalButton>
        <LocalButton variant="secondary" onClick={handleClose}>
          Отменить
        </LocalButton>
      </Modal.Footer>
    </Modal>
  );
};
