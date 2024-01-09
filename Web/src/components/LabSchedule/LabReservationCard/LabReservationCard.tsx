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

  return (
    <div className={style.reservationOverlay} style={{ display: showReservationModal ? 'block' : 'none' }}>
      <div className={style.reservationCardModal}>
        <div className={style.reservationCardContent}>
          <p>{selectedReservation?.timeStart} - {selectedReservation?.timeEnd}</p>
          <p>{selectedReservation?.theme}</p>
          <p>{selectedReservation?.reservor.firstName}</p>
          <p>{selectedReservation?.description}</p>
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
