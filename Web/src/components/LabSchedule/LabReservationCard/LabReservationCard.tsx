import React, { useState } from 'react';

import { LabReservation, UserRole } from '../../../../api';
import { Button as LocalButton } from '../../Button/Button';
import { userStore } from '../../../stores';
import { apiService } from '../../../services';
import { EditLabReservation } from '../EditLabReservation/EditLabReservation';

import style from './LabReservationCard.module.scss';

interface LabReservationCardProps {
  selectedReservation: LabReservation;
  showReservationModal: boolean;
  setShowReservationModal: (arg: boolean) => void;
  updateTable: () => void;
}

export const LabReservationCard: React.FC<LabReservationCardProps> = ({
  selectedReservation,
  showReservationModal,
  setShowReservationModal,
  updateTable,
}) => {
  const [showEditReservationModal, setShowEditReservationModal] = useState(false);
  const handleCloseReservationModal = () => {
    setShowReservationModal(false);
  };
    
  const handleDeleteReservationModal = async () => {
    const response = await apiService.deleteLabReservation(selectedReservation?.id, selectedReservation?.reservor?.id)
    if (!(response instanceof Error)) {
      updateTable();
    }
    handleCloseReservationModal();
  };

  const handleEditReservationButtonClick = () => {
    setShowEditReservationModal(true);
    handleCloseReservationModal();
  };

  const handleCloseEditreservationsModal = () => {
    setShowEditReservationModal(false);
  };
  const resTimeStart = new Date(selectedReservation?.timeStart);
  const resTimeEnd = new Date(selectedReservation?.timeEnd);

  return (
    <div className={style.reservationOverlay} style={{ display: showReservationModal ? 'block' : 'none' }}>
      <div className={style.reservationCardModal}>
        <div className={style.reservationCardContent}>
          <p className={style.time}>{resTimeStart.getDate().toString().padStart(2, '0')}.{(resTimeStart.getMonth() + 1).toString().padStart(2, '0')}
          {" "}{resTimeStart.getHours().toString().padStart(2, '0')}:{resTimeStart.getMinutes().toString().padStart(2, '0')}-
          {resTimeEnd.getHours()}:{resTimeEnd.getMinutes().toString().padStart(2, '0')}</p>
          <p className={style.theme}>{selectedReservation?.theme}</p>
          <p className={style.reservor}>{selectedReservation?.reservor.firstName}</p>
          <div className={style.description}>
            <p className={style.theme}>Описание:</p>
            <p className={style.description}>{selectedReservation?.description}</p>
          </div>
        </div>
        <LocalButton variant="secondary" onClick={handleCloseReservationModal}>
          Закрыть
        </LocalButton>
        {(userStore.user?.role === UserRole.Admin || (userStore.user?.id === selectedReservation?.id && userStore.user?.role === UserRole.Teacher)) &&
          <LocalButton className={style.deleteButton} variant="secondary" onClick={handleDeleteReservationModal}>
            Удалить
          </LocalButton>}
        {(userStore.user?.role === UserRole.Admin || (userStore.user?.id === selectedReservation?.id && userStore.user?.role === UserRole.Teacher)) &&
          <LocalButton className={style.editButton} variant="secondary" onClick={handleEditReservationButtonClick}>
            Изменить
          </LocalButton>}
        <EditLabReservation
          show={showEditReservationModal}
          handleClose={handleCloseEditreservationsModal}
          selectedReservation = {selectedReservation}
          updateTable = {updateTable}
        />
      </div>
    </div>
  );
};
