import { PartialType } from '@nestjs/mapped-types';
import { CreateJuegoDto } from './create-juego.dto';

// PartialType hace que nombre, imagen y categoria sean todos opcionales,
// tal como quedó en el contrato: PATCH acepta "cualquier subconjunto de los campos".
export class UpdateJuegoDto extends PartialType(CreateJuegoDto) {}
